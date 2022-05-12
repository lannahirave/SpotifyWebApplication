using System.Data;
using System.Dynamic;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SpotifyWebApplication.Controllers;

public class FiltersController : Controller
{
    private readonly spotifyContext _context; //NEVER USED BUT WE NEVER GIVE UP
    private readonly string _connectionString;

    public FiltersController(spotifyContext context)
    {
        _context = context;
        _connectionString = context.Database.GetConnectionString();
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    // POST: Filters/Filter1
    public async Task<IActionResult> Filter1(string songName)
    {
        //Знайти артистів, яким належить альбом, що містить пісню з такою назвою:
        if (songName is null) return RedirectToAction("Index");
        // DAPPER

        using IDbConnection db = new SqlConnection(_connectionString);
        var artists = await
            db.QueryAsync<Artist>(
                "SELECT * FROM Artists WHERE id IN (SELECT ArtistId FROM Albums WHERE id in (SELECT AlbumId FROM Songs WHERE Name=@songName));",
                new {songName});
        return Results(artists);
    }

    // POST: Filters/Filter2
    public async Task<IActionResult> Filter2(string min02, string sec02)
    {
        //Знайти альбоми, у яких середня довжина пісні більше
        if (min02 is null || sec02 is null) return RedirectToAction("Index");

        var time = Convert.ToInt32(min02) * 60 + Convert.ToInt32(sec02);
        using IDbConnection db = new SqlConnection(_connectionString);
        var albums = await
            db.QueryAsync<Album>(
                @"SELECT *
        FROM albums
        WHERE
            (SELECT AVG(duration)
        FROM songs
        WHERE albumId=albums.Id) > @time",
                new {time});
        return Results(albums: albums);
    }

    // POST: Filters/Filter3
    public async Task<IActionResult> Filter3(string min03, string sec03)
    {
        //Знайти плейлісти, у яких довжина більше
        if (min03 is null || sec03 is null) return RedirectToAction("Index");

        var time = Convert.ToInt32(min03) * 60 + Convert.ToInt32(sec03);
        using IDbConnection db = new SqlConnection(_connectionString);
        var playlists = await
            db.QueryAsync<Playlist>(
                @"SELECT p.*
FROM playlists p
JOIN Playlists_songs ps ON p.id=ps.PlaylistId
JOIN Songs s ON ps.SongId=s.id
GROUP BY p.id,
         p.Name,
         p.Description,
         p.PhotoLink
HAVING SUM(duration) > @time
ORDER BY p.Name;",
                new {time});
        return Results(playlists: playlists);
    }

    public async Task<IActionResult> Filter4(string number)
    {
        //Знайти пісні, у яких кількість артистів більше number
        if (number is null) return RedirectToAction("Index");

        var amount = Convert.ToInt32(number);
        using IDbConnection db = new SqlConnection(_connectionString);
        var songs = await
            db.QueryAsync<Song, Album, Song>(
                @"SELECT s.id, s.AlbumId, s.Duration, s.Name, a1.id as AlbId, a1.ArtistId, a1.Name, a1.PhotoLink, a1.PublisherId, a1.ReleaseDate
      FROM Songs AS s
      INNER JOIN Albums AS a1 ON s.AlbumId = a1.id
      WHERE (
          SELECT COUNT(*)
          FROM Artists_songs AS a
          INNER JOIN Artists AS a0 ON a.ArtistId = a0.id
          WHERE s.id = a.SongId) > @amount",
                (song, album) =>
                {
                    song.Album = album;
                    return song;
                },
                new {amount},
                splitOn: "AlbId");
        return Results(songs: songs);
    }

    public async Task<IActionResult> Filter5(string publisherName)
    {
        if (publisherName is null) return RedirectToAction("Index");
        using IDbConnection db = new SqlConnection(_connectionString);
        var albums = await
            db.QueryAsync<Album, Publisher, Album>(
                @"SELECT a.*, p.id as PubId, p.Name
      FROM Albums AS a
      INNER JOIN Publishers AS p ON a.PublisherId = p.id
      WHERE a.PublisherId = (SELECT TOP(1) p.id
      FROM Publishers AS p
      WHERE LOWER(p.Name) = @publisherName);",
                (album, publisher) =>
                {
                    album.Publisher = publisher;
                    return album;
                },
                new {publisherName},
                splitOn: "PubId");

        return Results(albums: albums);
    }

    public async Task<IActionResult> Filter6(string amountSongs)
    {
        // Знайти усіх артистів, у яких кількість пісень
        if (amountSongs is null) return RedirectToAction("Index");
        var amount = Convert.ToInt32(amountSongs);
        using IDbConnection db = new SqlConnection(_connectionString);
        var artists = await
            db.QueryAsync<Artist>(
                @"SELECT id, Name, PhotoLink, RankOnSpotify
FROM
  (SELECT a.*,
          Count(a.Name) AS ams
   FROM Artists a
   JOIN Artists_songs arts ON a.id = arts.ArtistId
   JOIN Songs s ON arts.SongId = s.id
   GROUP BY a.Id, a.Name, a.PhotoLink, a.RankOnSpotify) x
WHERE x.ams = @amount;",
                new {amount});

        return Results(artists);
    }

    public async Task<IActionResult> Filter7(string artistName)
    {
        // Знайти усі плейлісти, в яких є всі пісні автора 
        if (artistName is null) return RedirectToAction("Index");
        using IDbConnection db = new SqlConnection(_connectionString);
        var artist = await
            db.QueryFirstOrDefaultAsync<Artist>(
                @"SELECT * FROM Artists WHERE Name=@artistName",
                new {artistName});
        if (artist is null) return Results(null, null, null, null);
        var playlists = await db.QueryAsync<Playlist>(@"

WITH artistSongIds AS
         (SELECT arts.SongId as songs from Artists_songs arts WHERE arts.artistId=@artistId)
SELECT *
FROM playlists
WHERE Not exists
    (SELECT *
     FROM artistSongIds
     WHERE not exists
         (SELECT *
          FROM playlists_songs pls
          WHERE pls.songid = artistSongIds.songs
            AND pls.playlistid = playlists.id ))",
            new {artistId = artist.Id}
        );

        return Results(playlists: playlists);
    }

    public async Task<IActionResult> Filter8(string numberSongs, string numberArtists)
    {
        // знайти альбоми, у яких numberSongs пісень з к-стю артистів > numberArtists
        if (numberSongs is null || numberArtists is null) return RedirectToAction("Index");
        var amountSongs = Convert.ToInt32(numberSongs);
        var amountArtists = Convert.ToInt32(numberArtists);

        using IDbConnection db = new SqlConnection(_connectionString);
        var albums = await db.QueryAsync<Album>(@"WITH songsWithManyArtists AS
  (SELECT *
   FROM songs
   WHERE
       (SELECT count(DISTINCT ArtistId)
        FROM Artists_songs
        WHERE SongId = songs.Id ) > @amountArtists )
SELECT Albums.*
FROM Albums
WHERE
    (SELECT count(DISTINCT songsWithManyArtists.id)
     FROM songsWithManyArtists
     WHERE songsWithManyArtists.AlbumId = Albums.Id ) > @amountSongs;", new {amountArtists, amountSongs});
        return Results(albums: albums);
    }

    public IActionResult Results(IEnumerable<Artist> artists = null, IEnumerable<Album> albums = null,
        IEnumerable<Song> songs = null, IEnumerable<Playlist> playlists = null)
    {
        dynamic myModel = new ExpandoObject();
        myModel.Songs = songs;
        myModel.Albums = albums;
        myModel.Artists = artists;
        myModel.Playlists = playlists;
        return View("Results", myModel);
    }
}
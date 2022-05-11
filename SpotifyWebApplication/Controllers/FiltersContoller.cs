using System.Data;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpotifyWebApplication.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace SpotifyWebApplication.Controllers;

public class FiltersController : Controller
{
    private readonly spotifyContext _context;
    private readonly string _connectionString;

    public FiltersController(spotifyContext context)
    {
        _context = context;
        _connectionString = context.Database.GetConnectionString();
    }


    // GET
    public IActionResult Index()
    {
        var classes = new List<object>() {"Артисти", "Альбоми", "Пісні", "Релізери", "Плейлісти"};
        @ViewBag.ClassesForFirstFilter = new SelectList(classes);
        return View();
    }

    // POST: Filters/Filter1
    public async Task<IActionResult> Filter1(string filter01)
    {
        if (filter01 is null) return RedirectToAction("Index");
        // DAPPER

        using IDbConnection db = new SqlConnection(_connectionString);
        var artists = await
            db.QueryAsync<Artist>(
                "SELECT * FROM Artists WHERE id IN (SELECT ArtistId FROM Albums WHERE id in (SELECT AlbumId FROM Songs WHERE Name=@filter01));",
                new {filter01});
        return Results(artists, null, null, null);
    }

    // POST: Filters/Filter2
    public async Task<IActionResult> Filter2(string filter02m, string filter02s)
    {
        //Знайти альбоми, у яких середня довжина пісні більше
        if (filter02m is null && filter02s is null) return RedirectToAction("Index");

        int time = Convert.ToInt32(filter02m) * 60 + Convert.ToInt32(filter02s);
        using IDbConnection db = new SqlConnection(_connectionString);
        var albums = await
            db.QueryAsync<Album>(
                "select * from albums where (select AVG(duration) from songs where albumId=albums.Id) > @time",
                new {time});
        return Results(null, albums, null, null);
    }

    // POST: Filters/Filter3
    public async Task<IActionResult> Filter3(string filter03m, string filter03s)
    {
        //Знайти плейлісти, у яких довжина більше
        if (filter03m is null && filter03s is null) return RedirectToAction("Index");

        int time = Convert.ToInt32(filter03m) * 60 + Convert.ToInt32(filter03s);
        using IDbConnection db = new SqlConnection(_connectionString);
        var playlists = await
            db.QueryAsync<Playlist>(
                @"Select p.* from playlists p
        join Playlists_songs ps on p.id=ps.PlaylistId
        join Songs s on ps.SongId=s.id
        group by p.id, p.Name, p.Description, p.PhotoLink
            HAVING SUM(duration) > @time
        order by p.Name;",
                new {time});
        return Results(null, null, null, playlists);
    }

    public async Task<IActionResult> Filter4(string number)
    {
        //Знайти пісні, у яких кількість артистів більше number
        if (number is null) return RedirectToAction("Index");

        int amount = Convert.ToInt32(number);
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
        return Results(null, null, songs, null);
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

        return Results(null, albums, null, null);
    }

    public IActionResult Results(IEnumerable<Artist> artists, IEnumerable<Album> albums, IEnumerable<Song> songs,
        IEnumerable<Playlist> playlists)
    {
        dynamic myModel = new ExpandoObject();
        myModel.Songs = songs ?? null;
        myModel.Albums = albums ?? null;
        myModel.Artists = artists ?? null;
        myModel.Playlists = playlists ?? null;
        return View("Results", myModel);
    }
}
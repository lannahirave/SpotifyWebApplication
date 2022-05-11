using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SpotifyWebApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChartsController : ControllerBase
{
    private readonly spotifyContext _context;

    public ChartsController(spotifyContext context)
    {
        _context = context;
    }

    [HttpGet("JsonDataArtistAlbums")]
    public JsonResult JsonDataArtistAlbums()
    {
        var artists = _context.Artists.ToList();
        var artistAlbum = new List<object>();
        artistAlbum.Add(new[] {"Артист", "Кількість альбомів"});
        foreach (var artist in artists)
            artistAlbum.Add(new object[] {artist.Name, _context.Albums.Count(c => c.ArtistId == artist.Id)});
        return new JsonResult(artistAlbum);
    }

    [HttpGet("JsonDataPublishersAlbums")]
    public JsonResult JsonDataPublishersAlbums()
    {
        var publishers = _context.Publishers.ToList();
        var publisherAlbum = new List<object>();
        publisherAlbum.Add(new[] {"Релізер", "Кількість альбомів"});
        foreach (var publisher in publishers)
            publisherAlbum.Add(new object[]
            {
                publisher.Name, _context.Albums
                    .Count(c => c.PublisherId == publisher.Id)
            });
        return new JsonResult(publisherAlbum);
    }
}
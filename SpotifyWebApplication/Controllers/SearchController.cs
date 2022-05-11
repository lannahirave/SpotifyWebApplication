using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SpotifyWebApplication.Controllers;

public class SearchController : Controller
{
    private readonly spotifyContext _context;

    public SearchController(spotifyContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }

    // GET: SEARCH/
    public async Task<IActionResult> Search(string searchValue)
    {
        if (searchValue is null || searchValue.Length == 0) return RedirectToAction("Index", "Home");
        var songs = await _context.Songs.Where(a => a.Name.Contains(searchValue))
            .Include(d => d.Album).Include(a => a.Artists).ToListAsync();
        var albums = await _context.Albums.Where(a => a.Name.Contains(searchValue))
            .Include(d => d.Publisher).Include(a => a.Artist).ToListAsync();
        var artists = await _context.Artists.Where(a => a.Name.Contains(searchValue)).ToListAsync();
        var playlists = await _context.Playlists.Where(a => a.Name.Contains(searchValue)).ToListAsync();
        dynamic myModel = new ExpandoObject();
        myModel.Songs = songs;
        myModel.Albums = albums;
        myModel.Artists = artists;
        myModel.Playlists = playlists;
        return View(myModel);
    }
}
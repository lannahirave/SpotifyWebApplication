using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpotifyWebApplication.Models;

namespace SpotifyWebApplication.Controllers
{
    public class SearchController : Controller
    {
        private readonly spotifyContext _context;

        public SearchController(spotifyContext context)
        {
            _context = context;
        }

        // GET
        public async Task<IActionResult> Index(string searchValue)
        {
            if (searchValue is null || searchValue.Length == 0) return RedirectToAction("Index", "Home");
            var songs = await _context.Songs.Where(a => a.Name.Contains(searchValue)).Include(d => d.Album).ToListAsync();
            var albums = await _context.Albums.Where(a => a.Name.Contains(searchValue)).Include(d => d.Publisher).ToListAsync();
            var artists = await _context.Artists.Where(a => a.Name.Contains(searchValue)).ToListAsync();
            return View();
        }
    }
}
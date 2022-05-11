using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpotifyWebApplication;

namespace SpotifyWebApplication.Controllers;

public class AlbumsController : Controller
{
    private readonly spotifyContext _context;

    public AlbumsController(spotifyContext context)
    {
        _context = context;
    }

    // GET: Albums
    /*public async Task<IActionResult> Index()
    {
        var spotifyContext = _context.Albums.Include(a => a.Artist).Include(a => a.Publisher);
        return View(await spotifyContext.ToListAsync());
    } */
    public async Task<IActionResult> Index(int? id, string name)
    {
        if (id == null)
        {
            //return RedirectToAction("Artists", "Index"); 
            var spotifyContext = _context.Albums.Include(a => a.Artist).Include(a => a.Publisher);
            ViewBag.LinkToImage = "https://i.giphy.com/media/blSTtZehjAZ8I/giphy.webp";
            return View(await spotifyContext.ToListAsync());
        }

        // finding albums by artist
        ViewBag.ArtistId = id;
        ViewBag.ArtistName = name;
        var artist = await _context.Artists.FindAsync(id);
        ViewBag.LinkToImage = artist!.PhotoLink;
        var albumsByArtist = _context.Albums.Where(a => a.ArtistId == id)
            .Include(a => a.Artist).Include(a => a.Publisher);
        return View(await albumsByArtist.ToListAsync());
    }

    // GET: Albums/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var album = await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Publisher)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (album == null) return NotFound();

        //return View(album);
        return RedirectToAction("Index", "Songs", new {id = album.Id, name = album.Name});
    }

    // GET: Albums/Create
    public IActionResult Create(int? artistId)
    {
        if (artistId is null) return NotFound();
        var artist = _context.Artists.Find(artistId);
        if (artist is null) return NotFound();
        ViewBag.ArtistId = artistId;
        ViewBag.ArtistName = artist.Name;
        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
        return View();
    }

    // POST: Albums/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int artistId,
        [Bind("Id,Name,ReleaseDate,PublisherId,PhotoLink,ArtistId")] Album album)
    {
        album.ArtistId = artistId;
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "Albums",
                new {id = artistId, name = _context.Artists.FirstOrDefault(c => c.Id == artistId)?.Name});
        {
            _context.Add(album);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Albums",
                new {id = artistId, name = _context.Artists.FirstOrDefault(c => c.Id == artistId)?.Name});
        }
        //ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name", album.ArtistId);
        //ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", album.PublisherId);
        //return View(album);
    }

    // GET: Albums/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var album = await _context.Albums.FindAsync(id);
        if (album == null) return NotFound();
        ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name", album.ArtistId);
        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", album.PublisherId);
        var artist = await _context.Artists.FindAsync(album.ArtistId);
        @ViewBag.AlbumName = album.Name;
        @ViewBag.ArtistName = artist!.Name;
        @ViewBag.artid = artist.Id;
        return View(album);
    }

    // POST: Albums/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Name,ReleaseDate,PublisherId,PhotoLink,ArtistId")] Album album)
    {
        if (id != album.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(album);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(album.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction("Index", "Albums",
                new {id = album.ArtistId, name = _context.Artists.FirstOrDefault(c => c.Id == album.ArtistId)?.Name});
        }

        ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name", album.ArtistId);
        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", album.PublisherId);
        //return View(album);
        return RedirectToAction("Index", "Albums",
            new {id = album.ArtistId, name = _context.Artists.FirstOrDefault(c => c.Id == album.ArtistId)?.Name});
    }

    // GET: Albums/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var album = await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Publisher)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (album == null) return NotFound();
        var artist = await _context.Artists.FindAsync(album.ArtistId);
        @ViewBag.AlbumName = album.Name;
        @ViewBag.ArtistName = artist!.Name;
        @ViewBag.artid = artist.Id;
        return View(album);
    }

    // POST: Albums/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album is null) return NotFound();
        var artistId = album.ArtistId;
        var artist = await _context.Artists.FindAsync(artistId);
        var artistName = artist!.Name;
        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();
        //return RedirectToAction(nameof(Index));
        return RedirectToAction("Index", "Albums", new {id = artistId, name = artistName});
    }


    private bool AlbumExists(int id)
    {
        return _context.Albums.Any(e => e.Id == id);
    }
}
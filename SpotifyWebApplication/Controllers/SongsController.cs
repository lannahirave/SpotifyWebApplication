using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpotifyWebApplication;

namespace SpotifyWebApplication.Controllers
{
    public class SongsController : Controller
    {
        private readonly spotifyContext _context;

        public SongsController(spotifyContext context)
        {
            _context = context;
        }

        // GET: Songs
        public async Task<IActionResult> Index(int? id, string? name)
        {
            //var spotifyContext = _context.Songs.Include(s => s.Album);
            //return View(await spotifyContext.ToListAsync());
            if (id == null)
            {
                //return RedirectToAction("Artists", "Index"); 
                var spotifyContext = _context.Songs.Include(a => a.Album);
                return View(await spotifyContext.ToListAsync());
            }
            // finding songs by album
            ViewBag.AlbumId = id;
            ViewBag.Name = name;
            var songsByAlbum = _context.Songs.Where(a => a.AlbumId == id).Include(a => a.Album);
            ViewBag.Count = songsByAlbum.Count();
            return View(await songsByAlbum.ToListAsync());
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Album)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // GET: Songs/Create
        public IActionResult Create(int albumId)
        {
            //ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name");
            ViewBag.AlbumId = albumId;
            ViewBag.AlbumName = _context.Albums.Where(a => a.Id == albumId).FirstOrDefault().Name;
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int albumId, [Bind("Id,Name,Duration,AlbumId")] Song song)
        {
            song.AlbumId = albumId;
            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Songs", new {id = albumId, name = _context.Albums.Where(b => b.Id == albumId).FirstOrDefault().Name});
            }
            //ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name", song.AlbumId);
            //return View(song);
            return RedirectToAction("Index", "Songs", new { id = albumId, name = _context.Albums.Where(b => b.Id == albumId).FirstOrDefault().Name });

        }

        // GET: Songs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name", song.AlbumId);
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Duration,AlbumId")] Song song)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(song.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name", song.AlbumId);
            return View(song);
        }

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Album)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}

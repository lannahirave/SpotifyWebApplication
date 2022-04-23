
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpotifyWebApplication.Models;

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
        public async Task<IActionResult> Index(int? id, string name)
        {
            if (id == null)
            {
                //return RedirectToAction("Artists", "Index"); 
                var spotifyContext = _context.Songs.Include(a => a.Album);
                return View(await spotifyContext.ToListAsync());
            }

            // finding songs by album
            ViewBag.AlbumId = id;
            ViewBag.AlbumName = name;
            ViewBag.state = "альбому";
            var songsByAlbum = _context.Songs.Where(a => a.AlbumId == id).Include(a => a.Album);
            ViewBag.Count = songsByAlbum.Count();
            return View(await songsByAlbum.ToListAsync());
        }

        //GET:  Songs/PlaylistDetails/5
        public async Task<IActionResult> PlaylistDetails(int? id)
        {
            if (id is null)
            {
                return RedirectToPage("Index", "Playlists");
            }

            var playlist = await _context.Playlists.FirstOrDefaultAsync(a => a.Id == id);
            if (playlist is null)
            {
                return RedirectToPage("Index", "Playlists");
            }

            var playsongs = await _context.PlaylistsSongs.Where(c => c.PlaylistId == id).ToListAsync();
            //if (playsongs.Count == 0) return View(await _context.Songs.Where(a => a.Id <= 5).ToListAsync());

            List<int> songsIds = new();
            foreach (var plsong in playsongs)
            {
                songsIds.Add(plsong.SongId);
            }

            var songs = await _context.Songs
                .Where(c => songsIds.Contains(c.Id)).Include(a => a.Album).ToListAsync();

            ViewBag.PlaylistId = id;
            ViewBag.songs = songs;
            ViewBag.PlaylistName = playlist.Name;
            return View(songs);
        }

        // GET: Songs/AddPlaylistSong/5
        public async Task<IActionResult> AddPlaylistSong(int? playlistId)
        {
            if (playlistId is null)
            {
                return NotFound();
            }
            //ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name");
            ViewBag.PlaylistId= playlistId;
            var playlist = await _context.Playlists.FindAsync(playlistId);
            if (playlist is null) return NotFound();
            ViewBag.PlaylistName = playlist.Name;
            ViewBag.songs = new MultiSelectList(_context.Songs, "Id", "Name");
            return View();
        }
        // POST: Songs/AddPlaylistSong/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPlaylistSong(int albumId, [Bind("Id, PlaylistId, SongId")] PlaylistsSong playsong)
        {
            if (ModelState.IsValid)
            {
                var song = await _context.Songs.FirstOrDefaultAsync(c => c.Id == playsong.SongId);
                if (song is null) return NotFound();
                var playlist = await _context.Playlists.FirstOrDefaultAsync(c => c.Id == playsong.PlaylistId);
                _context.Add(playsong);
                await _context.SaveChangesAsync();
                //return RedirectToAction("Index", "Songs", new {id = albumId, name = _context.Albums.Where(b => b.Id == albumId).FirstOrDefault().Name});
            }
            return RedirectToAction("PlaylistDetails", "Songs", new {id = playsong.PlaylistId});
            
        }

        // GET: Songs/DeletePlaylistSong/5
        public async Task<IActionResult> DeletePlaylistSong(int? playlistId, int? songId)
        {
            if (playlistId is null || songId is null) return NotFound();
            var playSong = await _context.PlaylistsSongs
                .FirstOrDefaultAsync(c => c.PlaylistId == playlistId && c.SongId == songId);
            if (playSong is null) return NotFound();
            _context.PlaylistsSongs.Remove(playSong);
            await _context.SaveChangesAsync();
            return RedirectToAction("PlaylistDetails", "Songs", new {id = playlistId});
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
            ViewBag.AlbumName = _context.Albums.FirstOrDefault(a => a.Id == albumId)?.Name;
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int albumId, [Bind("Id,Name,Duration,AlbumId")] Song song)
        {
            if (ModelState.IsValid)
            {
                song.AlbumId = albumId;
                var album = _context.Albums
                    .Include(c => c.Artist)
                    .FirstOrDefault(c => c.Id == albumId);
                if (album == null)
                    return NotFound();
                song.Artists.Add(album.Artist);
                _context.Add(song);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Songs", new {id = albumId, name = album.Name});
            }

            //ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name", song.AlbumId);
            //return View(song);
            return RedirectToAction("Index", "Songs",
                new {id = albumId, name = _context.Albums.FirstOrDefault(b => b.Id == albumId)?.Name});
        }

        // GET: Songs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs.Include(c => c.Artists).FirstOrDefaultAsync(c => c.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            ViewBag.Artists = new MultiSelectList(_context.Artists, "Id", "Name");
            var songEdit = new SongEdit
            {
                Id = song.Id,
                Name = song.Name,
                Duration = song.Duration,
                AlbumId = song.AlbumId,
                ArtistsIds = song.Artists.Select(c => c.Id).ToList()
            };

            return View(songEdit);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SongEdit songEdit)
        {
            if (id != songEdit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var song = await _context.Songs.Include(c => c.Artists).FirstOrDefaultAsync(d => d.Id == songEdit.Id);
                if (song is null)
                {
                    return NotFound();
                }

                song.Name = songEdit.Name;
                song.Duration = songEdit.Duration;
                var artists = await _context.Artists.Where(c => songEdit.ArtistsIds.Contains(c.Id)).ToListAsync();
                song.Artists = artists;
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

                var album = await _context.Albums.FirstOrDefaultAsync(c => c.Id == song.AlbumId);
                var albumName = album!.Name;
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index","Songs",  new {id = song.AlbumId, name=albumName});
            }

            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Name", songEdit.AlbumId);
            return View(songEdit);
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
            if (song is null) return NotFound();
            var albumId = song.AlbumId;
            var album = await _context.Albums.FirstOrDefaultAsync(c => c.Id == albumId);
            var albumName = album!.Name;
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Songs",  new {id = albumId, name=albumName});
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}
using System;
using System.Collections.Generic;

namespace SpotifyWebApplication
{
    public partial class PlaylistsSong
    {
        public int Id { get; set; }
        public int PlaylistId { get; set; }
        public int SongId { get; set; }
        public DateTimeOffset TimeSongAdded { get; set; }

        public virtual Playlist Playlist { get; set; }
        public virtual Song Song { get; set; }
    }
}

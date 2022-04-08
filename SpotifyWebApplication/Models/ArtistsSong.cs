using System;
using System.Collections.Generic;

namespace SpotifyWebApplication
{
    public partial class ArtistsSong
    {
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public int SongId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Song Song { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace SpotifyWebApplication
{
    public class ArtistsSong
    {
        public int Id { get; set; }
        public int ArtistId { get; set; }
        public int SongId { get; set; }

        public Artist Artist { get; set; }
        public Song Song { get; set; }
    }
}

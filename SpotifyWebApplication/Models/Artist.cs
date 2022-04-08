using System;
using System.Collections.Generic;

namespace SpotifyWebApplication
{
    public partial class Artist
    {
        public Artist()
        {
            Albums = new HashSet<Album>();
            ArtistsSongs = new HashSet<ArtistsSong>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoLink { get; set; }
        public int RankOnSpotify { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<ArtistsSong> ArtistsSongs { get; set; }
    }
}

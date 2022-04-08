using System;
using System.Collections.Generic;

namespace SpotifyWebApplication
{
    public partial class Song
    {
        public Song()
        {
            ArtistsSongs = new HashSet<ArtistsSong>();
            PlaylistsSongs = new HashSet<PlaylistsSong>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public int AlbumId { get; set; }

        public virtual Album Album { get; set; }
        public virtual ICollection<ArtistsSong> ArtistsSongs { get; set; }
        public virtual ICollection<PlaylistsSong> PlaylistsSongs { get; set; }
    }
}

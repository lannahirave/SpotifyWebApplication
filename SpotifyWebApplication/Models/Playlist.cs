using System;
using System.Collections.Generic;

namespace SpotifyWebApplication
{
    public partial class Playlist
    {
        public Playlist()
        {
            PlaylistsSongs = new HashSet<PlaylistsSong>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoLink { get; set; }

        public virtual ICollection<PlaylistsSong> PlaylistsSongs { get; set; }
    }
}

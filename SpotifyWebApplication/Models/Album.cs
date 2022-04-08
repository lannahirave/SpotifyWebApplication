using System;
using System.Collections.Generic;

namespace SpotifyWebApplication
{
    public partial class Album
    {
        public Album()
        {
            Songs = new HashSet<Song>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public int PublisherId { get; set; }
        public string PhotoLink { get; set; }
        public int ArtistId { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}

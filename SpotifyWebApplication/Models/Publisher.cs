using System;
using System.Collections.Generic;

namespace SpotifyWebApplication
{
    public partial class Publisher
    {
        public Publisher()
        {
            Albums = new HashSet<Album>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotifyWebApplication
{
    public class Song 
    {
        public Song()
        {
            //ArtistsSongs = new HashSet<ArtistsSong>();
            PlaylistsSongs = new HashSet<PlaylistsSong>();
        }

        public int Id { get; set; }
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string Name { get; set; }
        [Display(Name = "Довжина")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public int Duration { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        [ForeignKey("Albums")]
        public int AlbumId { get; set; }

        [Display(Name = "Альбом")]
        public Album Album { get; set; }
        public ICollection<PlaylistsSong> PlaylistsSongs { get; set; }
        public ICollection<Artist> Artists { get; set; } = new HashSet<Artist>();
    }
}

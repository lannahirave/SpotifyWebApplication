using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string Name { get; set; }
        [Display(Name = "Довжина")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public int Duration { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public int AlbumId { get; set; }

        [Display(Name = "Альбом")]
        public virtual Album Album { get; set; }
        public virtual ICollection<ArtistsSong> ArtistsSongs { get; set; }
        public virtual ICollection<PlaylistsSong> PlaylistsSongs { get; set; }
    }
}

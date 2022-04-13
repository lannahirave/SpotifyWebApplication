using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpotifyWebApplication
{
    public partial class Playlist
    {
        public Playlist()
        {
            PlaylistsSongs = new HashSet<PlaylistsSong>();
        }

        public int Id
        {
            get; set;
            
        }
        [Display(Name="Назва")]
        [MaxLength(255)]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string Name { get; set; }
        [Display(Name="Опис")]
        [MaxLength(255)]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string PhotoLink { get; set; }

        public virtual ICollection<PlaylistsSong> PlaylistsSongs { get; set; }
    }
}

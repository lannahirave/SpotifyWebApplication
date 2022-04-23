using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpotifyWebApplication
{
    public partial class Playlist
    {
        public Playlist()
        {
            //Songs = new HashSet<Song>();
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
        [Display(Name="Посилання на фото")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string PhotoLink { get; set; }

        //public virtual ICollection<PlaylistsSong> PlaylistsSongs { get; set; }
        public ICollection<Song> Songs { get; set; } = new HashSet<Song>();
    }
}

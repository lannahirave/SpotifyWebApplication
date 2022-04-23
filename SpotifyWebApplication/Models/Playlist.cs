using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpotifyWebApplication
{
    public partial class Playlist
    {
        public int Id { get; set; }
        [Display(Name="Назва")]
        [MaxLength(255)]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string Name { get; set; }
        [Display(Name="Опис")]
        [MaxLength(1024)]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string Description { get; set; }
        [Display(Name="Посилання на фото")]
        [Url(ErrorMessage = "Має бути посиланням.")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string PhotoLink { get; set; }
        public ICollection<Song> Songs { get; set; } = new HashSet<Song>();
    }
}

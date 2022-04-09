using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SpotifyWebApplication
{
    public partial class Album
    {
        public Album()
        {
            Songs = new HashSet<Song>();
        }

        public int Id { get; set; }
        [Display (Name="Назва")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string Name { get; set; }
        [Display(Name = "Реліз")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public DateTimeOffset ReleaseDate { get; set; }
        [Display(Name = "Лейбл")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public int PublisherId { get; set; }
        [Display(Name = "Посилання на фотографію")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string PhotoLink { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public int ArtistId { get; set; }
        [Display(Name = "Артист")]
        public virtual Artist Artist { get; set; }
        [Display(Name = "Лейбл")]
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}

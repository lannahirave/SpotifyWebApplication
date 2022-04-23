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

        public int Id { get; set;}
        [Display (Name="Назва")]
        [StringLength(255, ErrorMessage = "Занадто коротке або занадто довге.")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string Name { get; set; }
        [Display(Name = "Реліз")]
        [DataType(DataType.Date, ErrorMessage = "Має бути в форматі дати.")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public DateTimeOffset ReleaseDate { get; set; }
        [Display(Name = "Лейбл")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public int PublisherId { get; set; }
        [Display(Name = "Посилання на фотографію")]
        [Url(ErrorMessage = "Має бути посиланням.")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string PhotoLink { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        [Display(Name = "Артист")]
        public int ArtistId { get; set; }
        [Display(Name = "Артист")]
        public virtual Artist Artist { get; set; }
        [Display(Name = "Лейбл")]
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}

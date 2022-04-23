using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpotifyWebApplication
{
    public partial class Artist
    {
        public Artist()
        {
            Albums = new HashSet<Album>();
            //ArtistsSongs = new HashSet<ArtistsSong>();
        }

        public int Id { get; set; }
        [Display(Name ="Артист")]
        [StringLength(255, ErrorMessage = "Занадто коротке або занадто довге ім'я.")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string Name { get; set; }
        [Display(Name = "Посилання на фотографію")]
        [Url(ErrorMessage = "Повинно бути посиланням.")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string PhotoLink { get; set; }
        [Display(Name="Популярність на Spotify")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Має бути більше 0, не повторюватись.")]
        [Required(ErrorMessage ="Поле не повинно бути пустим.")]
        public int RankOnSpotify { get; set; }

        public ICollection<Album> Albums { get; set; }
        public ICollection<Song> Songs { get; set; } = new HashSet<Song>();
    }
}

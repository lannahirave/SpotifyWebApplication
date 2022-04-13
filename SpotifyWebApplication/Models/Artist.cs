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
            ArtistsSongs = new HashSet<ArtistsSong>();
        }

        public int Id { get; set; }
        [Display(Name ="Артист")]
        [MaxLength(255)]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string Name { get; set; }
        [Display(Name = "Посилання на фотографію")]
        [Required(ErrorMessage = "Поле не повинно бути пустим.")]
        public string PhotoLink { get; set; }
        [Display(Name="Популярність на Spotify")]
        [Required(ErrorMessage ="Поле не повинно бути пустим.")]
        public int RankOnSpotify { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<ArtistsSong> ArtistsSongs { get; set; }
    }
}

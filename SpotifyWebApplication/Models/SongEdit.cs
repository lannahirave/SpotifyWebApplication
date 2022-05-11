using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpotifyWebApplication.Models;

public class SongEdit
{
    public int Id { get; set; }

    [Display(Name = "Назва")]
    [StringLength(255, ErrorMessage = "Занадто коротке або занадто довге.")]
    [Required(ErrorMessage = "Поле не повинно бути пустим.")]
    public string Name { get; set; }

    [Display(Name = "Довжина")]
    [Range(1, int.MaxValue, ErrorMessage = "Повинно бути більше 0.")]
    [Required(ErrorMessage = "Поле не повинно бути пустим.")]
    public int Duration { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути пустим.")]
    public int AlbumId { get; set; }

    public List<int> ArtistsIds { get; set; } = new();
}
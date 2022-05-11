using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpotifyWebApplication;

public partial class Publisher
{
    public Publisher()
    {
        Albums = new HashSet<Album>();
    }

    public int Id { get; set; }

    [Display(Name = "Релізер")]
    [Required(ErrorMessage = "Поле не повинно бути пустим.")]
    public string Name { get; set; }

    public virtual ICollection<Album> Albums { get; set; }
}
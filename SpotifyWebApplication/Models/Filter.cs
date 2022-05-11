using System.ComponentModel.DataAnnotations;

namespace SpotifyWebApplication.Models;

public class Filter
{
    private Filter()
    {
    }

    [Required(ErrorMessage = "Не повинно бути пустим.")]
    [DataType(DataType.Text)]
    public string Text { get; set; }
}
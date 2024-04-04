using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class Country
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Country Name Required")]
    public string? Name { get; set; }

    [MaxLength(10, ErrorMessage = "Maximum Length for the ShortName is 10 Characters")]
    public string? ShortName { get; set; }
    public ICollection<Hotel>? Hotels { get; set; }
}

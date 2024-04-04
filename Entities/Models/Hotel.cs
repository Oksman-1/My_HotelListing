using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Hotel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Hotel Name Required")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Hotel Address is Required")]
    public string? Address { get; set; }
    public double Rating { get; set; }

    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }
    public Country? Country { get; set; }
}

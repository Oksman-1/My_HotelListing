using System.ComponentModel.DataAnnotations;

namespace Shared.DataTranferObjects;

public record CountryForManipulationDto
{
	public int Id { get; set; }

	[Required(ErrorMessage = "Country Name is Required")]
	public string? Name { get; set; }

	[Required(ErrorMessage = "Country Shortname is Required")]
	[MaxLength(10, ErrorMessage = "Maximum Length for the ShortName is 10 Characters")]
	public string? ShortName { get; set; }
	//public ICollection<Hotel>? Hotels { get; set; }
}

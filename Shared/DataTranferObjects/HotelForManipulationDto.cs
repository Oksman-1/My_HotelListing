using System.ComponentModel.DataAnnotations;

namespace Shared.DataTranferObjects;

public record HotelForManipulationDto
{
	[Required(ErrorMessage = "Hotel Name Required")]
	[MaxLength(30, ErrorMessage = "Maximum Length for the Hotel name is 30 Characters.")]
	public string? Name { get; init; }

	[Required(ErrorMessage = "Hotel Address Required")]
	[MaxLength(20, ErrorMessage = "Maximum Length for the Hotel Address is 20 Characters.")]
	public string? Address { get; init; }

	[Range(0.0, 5.0, ErrorMessage = "Rating is required and it should be in between 0.0 and 5.0 inclusive")]
	public double Rating { get; init; }

}


namespace Shared.DataTranferObjects;

public record CountryForCreationDto : CountryForManipulationDto
{
	public IEnumerable<HotelForCreationDto>? Hotels { get; init; }
}


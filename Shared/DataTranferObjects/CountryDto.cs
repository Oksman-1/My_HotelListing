namespace Shared.DataTranferObjects;

//[Serializable]
public record CountryDto
{
	public int Id { get; init; }
	public string? Name { get; init; }
	public string? FullCountryDesignation { get; init; }	
	public string? ShortName { get; init; }
}
	


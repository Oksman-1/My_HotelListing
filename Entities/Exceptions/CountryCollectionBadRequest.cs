namespace Entities.Exceptions;

public sealed class CountryCollectionBadRequest : BadRequestException
{
	public CountryCollectionBadRequest() : base("Company collection sent from a client is null.")
	{

	}
}

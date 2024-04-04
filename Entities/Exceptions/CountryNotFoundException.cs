namespace Entities.Exceptions;

public sealed class CountryNotFoundException : NotFoundException
{
	public CountryNotFoundException(int CountryId) : base($"The Country with id: {CountryId} doesn't exist in the database")
	{
	}
}

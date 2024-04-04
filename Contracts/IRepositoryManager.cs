namespace Contracts;

public interface IRepositoryManager
{
	ICountryRepository Country { get; }
	IHotelRepository Hotel { get; }
	Task SaveAsync();
}

using Contracts;
using System.Diagnostics;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
	private readonly DatabaseContext _dbContext;
	private readonly Lazy<ICountryRepository> _countryRepository;
	private readonly Lazy<IHotelRepository> _hotelRepository;

	public RepositoryManager(DatabaseContext dbContext)
	{
		_dbContext = dbContext;
		_countryRepository = new Lazy<ICountryRepository>(() => new CountryRepository(dbContext));	
		_hotelRepository = new Lazy<IHotelRepository>(() => new HotelRepository(dbContext));	
	}
	public ICountryRepository Country => _countryRepository.Value;

	public IHotelRepository Hotel => _hotelRepository.Value;

	public async Task SaveAsync() => await _dbContext.SaveChangesAsync();

	
	
}

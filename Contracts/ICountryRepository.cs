using Entities.Models;

namespace Contracts;

public interface ICountryRepository
{
	Task<IEnumerable<Country>> GetAllCountriesAsync(bool trackChanges);
	Task<Country> GetCountryAsync(int id, bool trackChanges);	
	void CreateCountry(Country country);	
	Task<IEnumerable<Country>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);	
	void DeleteCountry(Country country);



}

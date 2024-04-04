using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CountryRepository : RepositoryBase<Country>, ICountryRepository
{
	public CountryRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}
		
	public async Task<IEnumerable<Country>> GetAllCountriesAsync(bool trackChanges) => 
		await FindAll(trackChanges)
		.OrderBy(n => n.Id)
		.ToListAsync();

	public async Task<Country> GetCountryAsync(int CountryId, bool trackChanges) => 
		await FindByCondition(e => e.Id.Equals(CountryId), trackChanges)
		.SingleOrDefaultAsync();

	public void CreateCountry(Country country) => Create(country);

	public async Task<IEnumerable<Country>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges) => await FindByCondition(i => ids.Contains(i.Id), trackChanges).ToListAsync();

	public void DeleteCountry(Country country) => Delete(country);

	
}

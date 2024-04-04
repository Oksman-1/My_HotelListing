using Shared.DataTranferObjects;

namespace Service.Contracts;

public interface ICountryService
{
	Task<IEnumerable<CountryDto>> GetAllCountriesAsync(bool trackChanges);
	Task<CountryDto> GetCountryAsync(int CountryId, bool trackChanges);
	Task<CountryDto> CreateCountryAsync(CountryForCreationDto country);
	Task<IEnumerable<CountryDto>> GetByIdsAsync(IEnumerable<int> countryIds, bool trackChanges);
	Task<(IEnumerable<CountryDto> countries, string countryIds)> CreateCountryCollectionAsync(IEnumerable<CountryForCreationDto> countryCollection);
	Task DeleteCountryAsync(int CountryId, bool trackChanges);	
	Task UpdateCountryAsync(int CountryId, CountryForUpdateDto countryForUpdate, bool trackChanges);

}

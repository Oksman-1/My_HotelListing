using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTranferObjects;

namespace Service;

public sealed class CountryService : ICountryService
{
	private readonly IRepositoryManager _repository;
	private readonly ILoggerManager _logger;
	private readonly IMapper _mapper;

	public CountryService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
	{
		_repository = repository;
		_logger = logger;
		_mapper = mapper;	
	}

	public async Task<IEnumerable<CountryDto>> GetAllCountriesAsync(bool trackChanges)
	{		
		var countries = await _repository.Country.GetAllCountriesAsync(trackChanges);

		var countriesDto = _mapper.Map<IEnumerable<CountryDto>>(countries);	

		return countriesDto;
		
	}

	public async Task<CountryDto> GetCountryAsync(int CountryId, bool trackChanges)
	{
		var country = await GetCountryAndCheckIfItExists(CountryId, trackChanges);

		var countryDto = _mapper.Map<CountryDto>(country);	

		return countryDto;	
		
	}

	public async Task<CountryDto> CreateCountryAsync(CountryForCreationDto country)
	{
		var countryEntity = _mapper.Map<Country>(country);

		_repository.Country.CreateCountry(countryEntity);

		await _repository.SaveAsync();

		var countryToReturn = _mapper.Map<CountryDto>(countryEntity); 
		
		return countryToReturn;	
	}

	public async Task<IEnumerable<CountryDto>> GetByIdsAsync(IEnumerable<int> countryIds, bool trackChanges)
	{
		if (countryIds is null)
			throw new IdParametersBadRequestException();

		var countryEntities = await _repository.Country.GetByIdsAsync(countryIds, trackChanges);

		if (countryIds.Count() != countryEntities.Count())
			throw new CollectionByIdsBadRequestException();

		var countriesToReturn = _mapper.Map<IEnumerable<CountryDto>>(countryEntities);

		return countriesToReturn;	
	}

	public async Task<(IEnumerable<CountryDto> countries, string countryIds)> CreateCountryCollectionAsync(IEnumerable<CountryForCreationDto> countryCollection)
	{
		if(countryCollection is null)
			throw new CountryCollectionBadRequest();

		var countryEntitties = _mapper.Map <IEnumerable<Country>>(countryCollection);
		foreach(var country in countryEntitties)
		{
			_repository.Country.CreateCountry(country);	
		}
		await _repository.SaveAsync();

		var countryCollectionToReturn = _mapper.Map<IEnumerable<CountryDto>>(countryEntitties);

		var countryIDs = string.Join(",", countryCollectionToReturn.Select(c => c.Id));	

		return (countries:countryCollectionToReturn, countryIds:countryIDs);

	}

	public async Task DeleteCountryAsync(int CountryId, bool trackChanges)
	{	
		var country = await GetCountryAndCheckIfItExists(CountryId, trackChanges);

		_repository.Country.DeleteCountry(country);	

		await _repository.SaveAsync();
	}

	public async Task UpdateCountryAsync(int CountryId, CountryForUpdateDto countryForUpdate, bool trackChanges)
	{	
		var country = await GetCountryAndCheckIfItExists(CountryId, trackChanges);

		_mapper.Map(countryForUpdate, country);	

		await _repository.SaveAsync();	
	}

	private async Task<Country> GetCountryAndCheckIfItExists(int CountryId, bool trackChanges)
	{
		var country = await _repository.Country.GetCountryAsync(CountryId, trackChanges);

		if (country is null)
			throw new CountryNotFoundException(CountryId);

		return country;	
	}
}

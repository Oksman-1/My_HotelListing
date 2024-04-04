using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Service.Contracts;
using Shared.DataTranferObjects;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service;

public sealed class HotelService : IHotelService
{
	private readonly IRepositoryManager _repository;
	private readonly ILoggerManager _logger;
	private readonly IMapper _mapper;
	private readonly IHotelLinks _hotelLinks;	
	

	public HotelService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IHotelLinks hotelLinks)
	{
		_repository = repository;
		_logger = logger;
		_mapper = mapper;	
		_hotelLinks = hotelLinks;
		
	}

	public async Task<(LinkResponse linkResponse, MetaData metaData)> GetHotelsAsync(int countryId, LinkParameters linkParameters, bool trackChanges)
	{
		if (!linkParameters.hotelParameters.ValidRatingRange)
			throw new MaxRatingRangeBadRequestException();

		if(linkParameters.hotelParameters.HotelRating < 0 || linkParameters.hotelParameters.HotelRating > 5.0)
			throw new NegativeRatingBadRequestException();

		await CheckIfCountryExists(countryId, trackChanges);	

		var hotelsWithMetaData = await _repository.Hotel.GetHotelsAsync(countryId, linkParameters.hotelParameters, trackChanges);

		var hotelsDto = _mapper.Map<IEnumerable<HotelDto>>(hotelsWithMetaData);

		var links = _hotelLinks.TryGenerateLinks(hotelsDto, linkParameters.hotelParameters.Fields, countryId, linkParameters.Context);
	
		return (linkResponse: links, metaData: hotelsWithMetaData.MetaData);
	}

	public async Task<HotelDto> GetHotelAsync(int countryId, int hotelId, bool trackChanges)
	{		
		await CheckIfCountryExists(countryId, trackChanges);
		
		var hotelDb = await GetHotelForCountryAndCheckIfItExists(countryId, hotelId, trackChanges);	

		if (hotelDb is null)
		    throw new HotelNotFoundException(hotelId);

		var singleHotelDto = _mapper.Map<HotelDto>(hotelDb);	

		return singleHotelDto;	
	}

	public async Task<HotelDto> CreateHotelForCompamyAsync(int countryId, HotelForCreationDto hotelForCreation, bool trackChanges)
	{
		await CheckIfCountryExists(countryId, trackChanges);

		var hotelEntity = _mapper.Map<Hotel>(hotelForCreation);

		_repository.Hotel.CreateHotelForCountry(countryId, hotelEntity);
		await _repository.SaveAsync();

		var hotelToReturn = _mapper.Map<HotelDto>(hotelEntity);

		return hotelToReturn;
	}

	public async Task DeleteHotelForCountryAsync(int CountryId, int hotelId, bool trackChanges)
	{
		
		await CheckIfCountryExists(CountryId, trackChanges);

		var hotelDb = await GetHotelForCountryAndCheckIfItExists(CountryId, hotelId, trackChanges);

		_repository.Hotel.DeleteHotel(hotelDb);

		await _repository.SaveAsync();	
	}

	public async Task UpdateHotelForCountryAsync(int countryId, int hotelId, HotelForUpdateDto hotelForUpdate, bool countryTrackChanges, bool hotelTrackChanges)
	{
		
		await CheckIfCountryExists(countryId, countryTrackChanges);

		var hotelDb = await GetHotelForCountryAndCheckIfItExists(countryId, hotelId, hotelTrackChanges);

		_mapper.Map(hotelForUpdate, hotelDb);
		
		await _repository.SaveAsync();	
	}

	public async Task<(HotelForUpdateDto hotelToPatch, Hotel hotelEntity)> GetHotelForPatchAsync(int countryId, int hotelId, bool countryTrackChanges, bool hotelTrackChanges)
	{
		
		await CheckIfCountryExists(countryId, countryTrackChanges);

		var hotelDb = await GetHotelForCountryAndCheckIfItExists(countryId, hotelId, hotelTrackChanges);

		var hotelToPatch = _mapper.Map<HotelForUpdateDto>(hotelDb);

		return (hotelToPatch, hotelDb);

	}

	public async Task SaveChangesForPatchAsync(HotelForUpdateDto hotelToPatch, Hotel hotelEntity)
	{
		_mapper.Map(hotelToPatch, hotelEntity);

		await _repository.SaveAsync();
	}

	private async Task CheckIfCountryExists(int countryId, bool trackChanges)
	{
		var country = await _repository.Country.GetCountryAsync(countryId, trackChanges);

		if (country is null)
			throw new CountryNotFoundException(countryId);
	}

	private async Task<Hotel> GetHotelForCountryAndCheckIfItExists(int countryId, int hotelId, bool trackChanges)
	{
		var hotelForCountry = await _repository.Hotel.GetHotelAsync(countryId, hotelId, trackChanges);

		if (hotelForCountry is null)
			throw new HotelNotFoundException(hotelId);

		return hotelForCountry;	
	}	
}

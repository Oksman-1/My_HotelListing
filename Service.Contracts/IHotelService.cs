using Entities.LinkModels;
using Entities.Models;
using Shared.DataTranferObjects;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Service.Contracts;

public interface IHotelService
{
	Task<(LinkResponse linkResponse, MetaData metaData)> GetHotelsAsync(int countryId, LinkParameters linkParameters, bool trackChanges);
	Task<HotelDto> GetHotelAsync(int CountryId, int HotelId, bool trackChanges);
	Task<HotelDto> CreateHotelForCompamyAsync(int countryId, HotelForCreationDto hotelForCreation, bool trackChanges );
	Task DeleteHotelForCountryAsync(int CountryId, int hotelId, bool trackChanges);
	Task UpdateHotelForCountryAsync(int countryId, int hotelId, HotelForUpdateDto hotelForUpdate, bool countryTrackChanges, bool hotelTrackChanges);
	Task<(HotelForUpdateDto hotelToPatch, Hotel hotelEntity)> GetHotelForPatchAsync(int countryId, int hotelId, bool countryTrackChanges, bool hotelTrackChanges);
	Task SaveChangesForPatchAsync(HotelForUpdateDto hotelToPatch, Hotel hotelEntity);
}

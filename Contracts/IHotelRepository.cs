using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IHotelRepository
{
	Task<PagedList<Hotel>> GetHotelsAsync(int countryId, HotelParameters hotelParameters,bool trackChanges);	
	Task<Hotel> GetHotelAsync(int countryId, int hotelId, bool trackChanges);
	void CreateHotelForCountry(int countryId, Hotel hotel);	
	void DeleteHotel(Hotel hotel);

}

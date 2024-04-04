using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;

namespace Repository;

public class HotelRepository : RepositoryBase<Hotel>, IHotelRepository
{
	public HotelRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}
		
	public async Task<PagedList<Hotel>> GetHotelsAsync(int countryId, HotelParameters hotelParameters, bool trackChanges)
	{
		var hotels = await FindByCondition(h => h.CountryId == countryId, trackChanges)
			.FilterHotelsByRatingRange(hotelParameters.MinRating, hotelParameters.MaxRating) 
			//.FilterHotelByRating(hotelParameters.HotelRating)
			.Search(hotelParameters.SearchTerm)
			.Sort(hotelParameters.OrderBy)
			//.OrderBy(h => h.Id)
			.ToListAsync();	

		return PagedList<Hotel>.ToPagedList(hotels,hotelParameters.PageNumber,hotelParameters.PageSize);	
	}

	public async Task<Hotel> GetHotelAsync(int countryId, int Id, bool trackChanges) => 
		await FindByCondition(c => c.CountryId.Equals(countryId) && c.Id.Equals(Id), trackChanges)
		.SingleOrDefaultAsync();

	public void CreateHotelForCountry(int countryId, Hotel hotel)
	{
		hotel.CountryId = countryId;
		Create(hotel);
	}
	public void DeleteHotel(Hotel hotel) => Delete(hotel);
	
}

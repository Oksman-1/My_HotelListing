using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using Repository.Extensions.Utility;
using Entities.Models;

namespace Repository.Extensions;

public static class RepositoryHotelExtensions
{
	public static IQueryable<Hotel> FilterHotelsByRatingRange(this IQueryable<Hotel> hotels, double MinRating, double MaxRating) => hotels.Where(h => (h.Rating >= MinRating && h.Rating <= MaxRating));

	public static IQueryable<Hotel> FilterHotelByRating(this IQueryable<Hotel> hotels, double HotelRating) =>
		hotels.Where(h => h.Rating == HotelRating);

	public static IQueryable<Hotel> Search(this IQueryable<Hotel> hotels, string SearchTerm)
	{
		if(string.IsNullOrWhiteSpace(SearchTerm))
			return hotels;

		var lowerCaseTerm = SearchTerm.Trim().ToLower();

		return hotels.Where(h => h.Name.ToLower().Contains(lowerCaseTerm));
	}

	public static IQueryable<Hotel> Sort(this IQueryable<Hotel> hotels, string orderByQueryString)
	{
		if (string.IsNullOrWhiteSpace(orderByQueryString))	
			return hotels.OrderBy(e => e.Id);

	   var orderQuery = OrderQueryBuilder.CreateOrderQuery<Hotel>(orderByQueryString);	

		if (string.IsNullOrWhiteSpace(orderQuery))
			return hotels.OrderBy(e => e.Name);

		return hotels.OrderBy(orderQuery);	
	}


}

//(h => h.Rating.ToString().Contains(HotelRating.ToString()) && (h.Rating == HotelRating))
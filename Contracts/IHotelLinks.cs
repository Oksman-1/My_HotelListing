using Entities.LinkModels;
using Microsoft.AspNetCore.Http;
using Shared.DataTranferObjects;
namespace Contracts;


public interface IHotelLinks
{
	LinkResponse TryGenerateLinks(IEnumerable<HotelDto> hotelsDto, string fields, int companyId, HttpContext httpContext);
}

using Contracts;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.Net.Http.Headers;
using Shared.DataTranferObjects;


namespace CompanyEmployees.Utility;

public class HotelLinks : IHotelLinks
{
	private readonly LinkGenerator _linkGenerator;

	private readonly IDataShaper<HotelDto> _dataShaper;

	public Dictionary<string, MediaTypeHeaderValue> AcceptHeader { get; set; } = new Dictionary<string, MediaTypeHeaderValue>();

	public HotelLinks(LinkGenerator linkGenerator, IDataShaper<HotelDto> dataShaper)
	{
		_linkGenerator = linkGenerator;
		_dataShaper = dataShaper;
	}

	public LinkResponse TryGenerateLinks(IEnumerable<HotelDto> hotelsDto, string fields, int companyId, HttpContext httpContext)
	{
		var shapedHotels = ShapeData(hotelsDto, fields);

		if (ShouldGenerateLinks(httpContext))
			return ReturnLinkdedHotels(hotelsDto, fields, companyId, httpContext, shapedHotels);

		return ReturnShapedHotels(shapedHotels);
	}

	private List<Entity> ShapeData(IEnumerable<HotelDto> hotelsDto, string fields) => _dataShaper.ShapeData(hotelsDto, fields)
			.Select(e => e.Entity)
			.ToList();


	private bool ShouldGenerateLinks(HttpContext httpContext)
	{
		var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

		return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
	}

	private LinkResponse ReturnShapedHotels(List<Entity> shapedHotels) => new LinkResponse { ShapedEntities = shapedHotels };

	private LinkResponse ReturnLinkdedHotels(IEnumerable<HotelDto> hotelsDto,
	string fields, int companyId, HttpContext httpContext, List<Entity> shapedHotels)
	{
		var hotelDtoList = hotelsDto.ToList();

		for (var index = 0; index < hotelDtoList.Count(); index++)
		{
			var hotelLinks = CreateLinksForHotel(httpContext, companyId, hotelDtoList[index].Id, fields);
			shapedHotels[index].Add("Links", hotelLinks);
		}

		var hotelCollection = new LinkCollectionWrapper<Entity>(shapedHotels);
		var linkedHotels = CreateLinksForHotels(httpContext, hotelCollection);

		return new LinkResponse { HasLinks = true, LinkedEntities = linkedHotels };
	}

	private List<Link> CreateLinksForHotel(HttpContext httpContext, int companyId, int HotelId, string fields = "")
	{
		var links = new List<Link>
			{
				new Link(_linkGenerator.GetUriByAction(httpContext, "GetSingleHotel", values: new { companyId, HotelId, fields }),
				"self",
				"GET"),
				new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteHotelForCountry", values: new { companyId, HotelId }),
				"delete_hotel",
				"DELETE"),
				new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateHotelForCountry", values: new { companyId, HotelId }),
				"update_hotel",
				"PUT"),
				new Link(_linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateHotelForCountry", values: new { companyId, HotelId }),
				"partially_update_hotel",
				"PATCH")
			};
		return links;
	}

	private LinkCollectionWrapper<Entity> CreateLinksForHotels(HttpContext httpContext, LinkCollectionWrapper<Entity> hotelsWrapper)
	{
		hotelsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetHotelsForCountry", values: new { }),
				"self",
				"GET"));

		return hotelsWrapper;
	}
} 
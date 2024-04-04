using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace My_HotelListing.Presentation.Controllers;

[Route("api")]
[ApiController]
public class RootController : ControllerBase
{
	private readonly LinkGenerator _linkGenerator;

	public RootController(LinkGenerator linkGenerator) =>	_linkGenerator = linkGenerator;

	[HttpGet(Name = "GetRoot")]
	public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
	{
		if (mediaType.Contains("application/vnd.hotelier.apiroot"))
		{
			var list = new List<Link>
			{
				 new Link
				 {
					 Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new {}),
					 Rel = "self",
					 Method = "GET"
				 },
				 new Link
				 {
					 Href = _linkGenerator.GetUriByName(HttpContext, "GetCountries", new {}),
					 Rel = "countries",
					 Method = "GET"
				 },
				 new Link
				 {
					 Href = _linkGenerator.GetUriByName(HttpContext, "CreateCountry", new {}),
					 Rel = "create_country",
					 Method = "POST"
				 }

			};

			return Ok(list);
		}

		return NoContent();	
	}
}

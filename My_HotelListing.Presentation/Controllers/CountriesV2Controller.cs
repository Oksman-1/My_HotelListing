using Contracts;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace My_HotelListing.Presentation.Controllers;


[Route("api/countries")]
[ApiController]
[ApiExplorerSettings(GroupName = "v2")]
public class CountriesV2Controller : ControllerBase
{
	private readonly IServiceManager _service;
	private readonly ILoggerManager _logger;


	public CountriesV2Controller(IServiceManager service, ILoggerManager logger)
	{
		_service = service;
		_logger = logger;
	}

	[HttpGet(Name = "GetCountries")]
	public async Task<IActionResult> GetCoutries()
	{
		var countries = await _service.CountryService.GetAllCountriesAsync(trackChanges: false);
		var countriesV2 = countries.Select(c => $"{c.Name}--V2");
		_logger.LogInfo("Now in CountriesController V2.0!!");

		return Ok(countriesV2);

	}
}

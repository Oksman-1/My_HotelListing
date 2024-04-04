using Contracts;
using Entities;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_HotelListing.Presentation.ActionFilters;
using My_HotelListing.Presentation.ModelBinders;
using Service.Contracts;
using Shared.DataTranferObjects;

namespace My_HotelListing.Presentation.Controllers;

[Route("api/countries")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
//[ResponseCache(CacheProfileName = "120SecondsDuration")]
public class CountriesController : ControllerBase
{
	private readonly IServiceManager _service;
	private readonly ILoggerManager _logger;
	

	public CountriesController(IServiceManager service, ILoggerManager logger)
	{
		_service = service;
		_logger = logger;
		
	}

	[HttpGet(Name = "GetCountries")]
	//[Authorize(Roles = "Manager")]
	public async Task<IActionResult> GetCountries()
	{
		
		var countries = await _service.CountryService.GetAllCountriesAsync(trackChanges: false);
		_logger.LogWarn("Now in CountriesController V1.0 !!");			

		return Ok(countries);		
		
	}
	
	[HttpGet("{CountryId:int}", Name = "CountryById")]
	//[ResponseCache(Duration = 60)]
	[HttpCacheExpiration(CacheLocation = CacheLocation.Public,MaxAge = 60)]
	public async Task<IActionResult> GetCountry(int CountryId)
	{
		var country = await _service.CountryService.GetCountryAsync(CountryId, trackChanges: false);

		 return Ok(country);	
	}

	[HttpGet("collection/({CountryIds})", Name = "CountryCollection")]
	public async Task<IActionResult> GetCountryCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> CountryIds)
	{
		var countries = await _service.CountryService.GetByIdsAsync(CountryIds, trackChanges: false);

		return Ok(countries);

	}

	[HttpPost(Name = "CreateCountry")]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> CreateCountry([FromBody] CountryForCreationDto country)
	{		

		var createdCountry = await _service.CountryService.CreateCountryAsync(country);

		return CreatedAtRoute("CountryById", new {CountryId = createdCountry.Id}, createdCountry);

	}

	[HttpPost("collection")]
	public async Task<IActionResult> CreateCountryCollection([FromBody] IEnumerable<CountryForCreationDto> countryCollection)
	{
		var result = await _service.CountryService.CreateCountryCollectionAsync(countryCollection);

		return CreatedAtRoute("CountryCollection", new {result.countryIds}, result.countries);
	}

	[HttpDelete("{countryId:int}")]
	public async Task<IActionResult> DeleteCountry(int countryId)
	{
		await _service.CountryService.DeleteCountryAsync(countryId, trackChanges: false);	

		return NoContent();	
	}

	[HttpPut("{countryId:int}")]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> UpdateCountry(int countryId, [FromBody] CountryForUpdateDto country)
	{
		
		await _service.CountryService.UpdateCountryAsync(countryId,country,trackChanges:true);

		return NoContent();
	}

	[HttpOptions]
	public IActionResult GetCountriesOptions()
	{
		Response.Headers.Add("Allow", "GET, OPTIONS, POST, PUT, DELETE");

		return Ok();
	}
}



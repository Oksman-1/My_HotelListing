using CompanyEmployees.Presentation.ActionFilters;
using Entities;
using Entities.LinkModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using My_HotelListing.Presentation.ActionFilters;
using Service.Contracts;
using Shared.DataTranferObjects;
using Shared.RequestFeatures;
using System.ComponentModel.Design;
using System.Text.Json;

namespace My_HotelListing.Presentation.Controllers;

[Route("api/countries/{countryId}/hotels")]
[ApiController]
public class HotelsController : ControllerBase
{
	private readonly IServiceManager _service;

	public HotelsController(IServiceManager service)
	{
		_service = service;
	}

	[HttpGet]
	[HttpHead]
	[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
	public async Task<IActionResult> GetHotelsForEachCountry(int countryId, [FromQuery] HotelParameters hotelParameters)
	{
		var linkParams = new LinkParameters(hotelParameters, HttpContext);
		var result = await _service.HotelService.GetHotelsAsync(countryId, linkParams, trackChanges: false);
		
		Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(result.metaData));

		return result.linkResponse.HasLinks ? Ok(result.linkResponse.LinkedEntities) : Ok(result.linkResponse.ShapedEntities);
	}

	[HttpGet("{HotelId:int}", Name = "GetHotelForCountry")]
	public async Task<IActionResult> GetSingleHotel(int countryId, int HotelId) 
	{ 
		var hotel = await _service.HotelService.GetHotelAsync(countryId, HotelId, trackChanges: true);

		return Ok(hotel);	
	}

	[HttpPost]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> CreateHotelForCountry(int countryId, [FromBody] HotelForCreationDto hotel)
	{	

		var hotelToReturn = await _service.HotelService.CreateHotelForCompamyAsync(countryId, hotel, trackChanges: false);

		return CreatedAtRoute("GetHotelForCountry", new {countryId, HotelId = hotelToReturn.Id }, hotelToReturn);
	}

	[HttpDelete("{hotelId:int}")]
	public async Task<IActionResult> DeleteHotelForCountry(int countryId, int hotelId)
	{
		await _service.HotelService.DeleteHotelForCountryAsync(countryId, hotelId, trackChanges: false);

		return NoContent();	
	}

	[Authorize(Roles = "Manager")]
	[HttpPut("{hotelId:int}")]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> UpdateHotelForCountry(int countryId, int hotelId, [FromBody] HotelForUpdateDto hotel)
	{
		await _service.HotelService.UpdateHotelForCountryAsync(countryId, hotelId, hotel, countryTrackChanges: false, hotelTrackChanges: true);	 

		return NoContent();
	}

	[HttpPatch("{hotelId:int}")]
	public async Task<IActionResult> PartiallyUpdateHotelForCompany(int countryId, int hotelId, [FromBody] JsonPatchDocument<HotelForUpdateDto> patchDoc)
	{
		if (patchDoc is null)
			return BadRequest("patchDoc Object sent from Client is null");

		var result = await _service.HotelService.GetHotelForPatchAsync(countryId, hotelId, countryTrackChanges: false, hotelTrackChanges: true);

		patchDoc.ApplyTo(result.hotelToPatch, ModelState);
		TryValidateModel(result.hotelToPatch);

		if (!ModelState.IsValid)
			return UnprocessableEntity(ModelState);

		await _service.HotelService.SaveChangesForPatchAsync(result.hotelToPatch, result.hotelEntity);

		return NoContent();
	}
}


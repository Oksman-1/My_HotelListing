﻿using Microsoft.AspNetCore.Mvc;
using My_HotelListing.Presentation.ActionFilters;
using Service.Contracts;
using Shared.DataTranferObjects;

namespace My_HotelListing.Presentation.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController : ControllerBase
{
	private readonly IServiceManager _service;

	public TokenController(IServiceManager service) => _service = service;


	[HttpPost("refresh")]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
	{
		var tokenDtoToReturn = await _service.AuthenticationService.RefreshToken(tokenDto);

		return Ok(tokenDtoToReturn);
	}

}

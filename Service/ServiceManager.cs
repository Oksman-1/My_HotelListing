using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Shared.DataTranferObjects;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
	private readonly Lazy<ICountryService> _countryService;
	private readonly Lazy<IHotelService> _hotelService;
	private readonly Lazy<IAuthenticationService> _authenticationService;

	public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper, IHotelLinks hotelLinks, UserManager<User> userManager, IOptions<JwtConfiguration> configuration)
	{
		_countryService = new Lazy<ICountryService>(() => new CountryService(repositoryManager, logger, mapper));
		_hotelService = new Lazy<IHotelService>(() => new HotelService(repositoryManager, logger, mapper, hotelLinks));
		_authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, userManager, configuration));
	}
	public ICountryService CountryService => _countryService.Value;
	public IHotelService HotelService => _hotelService.Value;
	public IAuthenticationService AuthenticationService => _authenticationService.Value;
}

namespace Service.Contracts;

public interface IServiceManager
{
	ICountryService CountryService { get; }	
	IHotelService HotelService { get; }
	IAuthenticationService AuthenticationService { get; }	
}

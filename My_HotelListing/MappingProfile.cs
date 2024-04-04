using AutoMapper;
using Entities.Models;
using Shared.DataTranferObjects;

namespace My_HotelListing;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Country, CountryDto>()
			.ForMember(f => f.FullCountryDesignation, 
			opt => opt.MapFrom(m => string.Join('-',
			m.Name, m.ShortName)));

		CreateMap<Hotel, HotelDto>();

		CreateMap<CountryForCreationDto, Country>();	

		CreateMap<HotelForCreationDto, Hotel>();
		
		CreateMap<HotelForUpdateDto, Hotel>();

		CreateMap<CountryForUpdateDto, Country>();

		CreateMap<HotelForUpdateDto, Hotel>().ReverseMap();

		CreateMap<UserForRegistrationDto, User>();
	}

}

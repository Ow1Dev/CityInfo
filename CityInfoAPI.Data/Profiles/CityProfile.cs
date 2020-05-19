using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Data.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
            CreateMap<Entities.City, Models.CityDto>()
                .ForMember(
                    dest => dest.CityOfLanguages,
                    opt => opt.MapFrom(src => src.CityLanguages.Select(x => x.Language)
                    ));

            CreateMap<Models.CityOfCreationDto, Entities.City>().ReverseMap();
        }

    }
    
}

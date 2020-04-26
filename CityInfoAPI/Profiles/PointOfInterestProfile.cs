using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInteresForUpdatatingDto, Entities.PointOfInterest>();
            CreateMap<Models.PointOfInteresForUpdatatingDto, Entities.PointOfInterest>()
                .ReverseMap();
            
        }
    }
}

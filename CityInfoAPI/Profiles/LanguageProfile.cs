using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Profiles
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<Entities.Language, Models.LanguageDto>();
            CreateMap<Models.LanguageOfCreation, Entities.Language>();
            CreateMap<Models.LanguageOfUpdate, Entities.Language>();
            CreateMap<Entities.Language, Models.LanguageOfUpdate>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Entities
{
    public class CityLanguage
    {
        public int CityId { get; set; }
        public City City { get; set; }
        
        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }
}

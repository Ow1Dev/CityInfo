using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Models
{
    public class LanguageOfManipulation
    {
        [Required]
        public virtual string Name { get; set; }
    }
}

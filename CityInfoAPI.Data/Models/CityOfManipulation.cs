using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Data.Models
{
    public abstract class CityOfManipulationDto : IValidatableObject
    {
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Name == Description)
            {
                yield return new ValidationResult("Name and Description can not be the same");
            }
        }
    }
}

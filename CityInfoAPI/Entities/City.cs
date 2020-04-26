using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Entities
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }

        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
               = new List<PointOfInterest>();

        public ICollection<CityLanguage> CityLanguages { get; set; }
               = new List<CityLanguage>();
        // Vi kan ikke her umiddelbart se, om der er tale om en : En-til-Mange relation eller en 
        // Mange-til-Mange relation. Det kan vi kun se ved at se se på den anden af "relationen". 
        // I dette tilfælde filen CityLanguages.cs (og Language.cs). Ser vi på CityLanguages.cs filen 
        // kan vi iagttage, at der må være tale om en Mange-til-Mange releation, da denne fil har 2 styk :
        // En-til-Mange relationer "pegende ind imod sig". Én En-til-Mange relation fra City.cs og én 
        // En-til-Mange relation fra Language.cs. Og fra jeres Database kurser ved I, at en 
        // Mange-til_Mange relation kræver en samlingstabel, som samler to styk En-til-Mange relationer. 
    }
}

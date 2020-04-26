using AutoMapper;
using CityInfoAPI.Models;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ??
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        public IActionResult getCities( [FromQuery] string orderby, [FromQuery] bool GoDeeper = false)
        {
            var cityEntities = _cityInfoRepository.GetCities(orderby);

            if(!GoDeeper)
            {
                return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
            }
            else
            {
                return Ok(_mapper.Map<IEnumerable<CityDto>>(cityEntities));
            }
        }

        [HttpGet("{id}", Name = "GetCity")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }

        [HttpPost]
        public IActionResult CreateCity([FromBody] CityOfCreationDto cityOfCreation)
        {
            var FinalCity = _mapper.Map<Entities.City>(cityOfCreation);
            _cityInfoRepository.AddCity(FinalCity);
            _cityInfoRepository.Save();

            var CityToReturn = _mapper.Map<Models.CityDto>(FinalCity);
            return CreatedAtRoute("GetCity",
                new { id = CityToReturn.Id },
                CityToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCity(int id, [FromBody] CityOfUpdateDto city)
        {
            var cityFromRepo = _cityInfoRepository.GetCity(id, false);
            if (cityFromRepo == null)
                return NotFound();

            _mapper.Map(city, cityFromRepo);

            _cityInfoRepository.UpdateCity(cityFromRepo);
            _cityInfoRepository.Save();

            return NoContent();

        }

        [HttpPatch("{id}")]
        public IActionResult PatchCity(int id,
            [FromBody] JsonPatchDocument<CityOfUpdateDto> patchDoc)
        {
            var cityFromRepo = _cityInfoRepository.GetCity(id, false);
            if (cityFromRepo == null)
                return NotFound();

            var cityToPatch = _mapper.Map<CityOfUpdateDto>(cityFromRepo);

            patchDoc.ApplyTo(cityToPatch, ModelState);

            if (ModelState.IsValid)
                return BadRequest();

            if (!(TryValidateModel(cityToPatch)))
                return BadRequest(ModelState);

            _mapper.Map(cityToPatch, cityFromRepo);

            _cityInfoRepository.UpdateCity(cityFromRepo);
            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCity(int id)
        {
            var cityFromRepo = _cityInfoRepository.GetCity(id, false);
            if (cityFromRepo == null)
                return NotFound();

            _cityInfoRepository.DeleteCity(cityFromRepo);
            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpGet("{id}/languages")]
        public ActionResult<IEnumerable<LanguageDto>> GetLanguagesFromCity(int id)
        {
            var LanguagesFromRepo = _cityInfoRepository.GetLanguages(id);
            return Ok(_mapper.Map<IEnumerable<LanguageDto>>(LanguagesFromRepo));
        }

        [HttpGet("{id}/languages/{languageId}")]
        public ActionResult<LanguageDto> GetLanguageFromCity(int id, int languageId)
        {
            if (!_cityInfoRepository.CityExists(id))
                return NotFound();

            var LanguagesFromRepo = _cityInfoRepository.GetLanguageFromCity(id, languageId);
            return Ok(_mapper.Map<IEnumerable<LanguageDto>>(LanguagesFromRepo));
        }

        [HttpPost("{id}/languages")]
        public IActionResult AddLanguageFromCity(int id, [FromBody] LanguageCityForAddingDto languageCityForAdding)
        {
            if (!_cityInfoRepository.CityExists(id))
                return NotFound();

            if (!_cityInfoRepository.LanguageExists(id))
                return NotFound();

            _cityInfoRepository.AddLanguageToCity(id, languageCityForAdding.id);
            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}/languages/{languageId}")]
        public IActionResult RemoveLanguageFromCity(int id, int languageId)
        {
            var cityLanguageLinkFromRepo = _cityInfoRepository.GetCityLanguageLink(id, languageId);
            if (cityLanguageLinkFromRepo == null)
                return NotFound();

            _cityInfoRepository.RemoveLanguageToCity(cityLanguageLinkFromRepo);
            _cityInfoRepository.Save();

            return NoContent();
        }
    }
}

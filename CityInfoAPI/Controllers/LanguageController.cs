using AutoMapper;
using CityInfoAPI.Data.Models;
using CityInfoAPI.Data;
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
    [Route("api/languages")]
    public class LanguageController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public LanguageController(
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ??
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper)); 
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetLanguages()
        {
            var LanguagesFromRepo = _cityInfoRepository.GetLanguages();
            return Ok(_mapper.Map<IEnumerable<LanguageDto>>(LanguagesFromRepo)); 
        }

        [HttpGet("{LanguageId}", Name = "GetLanguages")]
        public ActionResult<CityDto> GetLanguage(int languageId)
        {
            var LanguagesFromRepo = _cityInfoRepository.GetLanguage(languageId);
            return Ok(_mapper.Map<LanguageDto>(LanguagesFromRepo));
        }


        [HttpPost]
        public IActionResult AddLanguages(LanguageOfCreation language)
        {
            var finalLanguage = _mapper.Map<Data.Entities.Language>(language);

            _cityInfoRepository.AddLanguage(finalLanguage);
            _cityInfoRepository.Save();

            var LaunguageToReturn = _mapper.Map<Data.Models.LanguageDto>(finalLanguage);
            return CreatedAtRoute("GetLanguages",
                new
                {
                    languageId = LaunguageToReturn.Id
                }, LaunguageToReturn);
        }
        
        [HttpPut("{id}")]
        public IActionResult UpdateLanguages(int id, [FromBody] LanguageOfUpdate language)
        {
            var languageFromRepo = _cityInfoRepository.GetLanguage(id);
            if (languageFromRepo == null)
                return NotFound();

            _mapper.Map(language, languageFromRepo);

            _cityInfoRepository.UpdateLanguage(languageFromRepo);
            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchLanguage(int id,
            [FromBody] JsonPatchDocument<LanguageOfUpdate> patchDoc)
        {
            var languageFromRepo = _cityInfoRepository.GetLanguage(id);
            if (languageFromRepo == null)
                return NotFound();

            var languageToPatch = _mapper.Map<LanguageOfUpdate>(languageFromRepo);

            patchDoc.ApplyTo(languageToPatch, ModelState);

            if(!ModelState.IsValid)
                return BadRequest();

            if (!(TryValidateModel(languageToPatch)))
                return BadRequest(ModelState);

            _mapper.Map(languageToPatch, languageFromRepo);
            
            _cityInfoRepository.UpdateLanguage(languageFromRepo);
            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpDelete("{LanguageId}")]
        public IActionResult RemoveLanguages(int languageId)
        {
            var LanguageFromRepo = _cityInfoRepository.GetLanguage(languageId);
            if (LanguageFromRepo == null)
                return NotFound();

            _cityInfoRepository.DeleteLanguages(LanguageFromRepo);
            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpGet("{LanguageId}/cities")]
        public ActionResult<IEnumerable<CityDto>> GetCitiesFromLanguages(int languageId)
        {
            if(!_cityInfoRepository.LanguageExists(languageId))
            {
                return BadRequest();
            }

            var CitiesFromRepo = _cityInfoRepository.GetCitiesFromLanguages(languageId);
            return Ok(_mapper.Map<IEnumerable<CityDto>>(CitiesFromRepo));
        }
    }
}

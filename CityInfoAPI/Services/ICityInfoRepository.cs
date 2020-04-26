using CityInfoAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities(string orderby = "");
        City GetCity(int cityId, bool includePointsOfInterest);
        void AddCity(City city);
        void UpdateCity(City city);

        IEnumerable<PointOfInterest> GetPointOfInterestForCity(int cityId);
        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);

        bool CityExists(int cityId);

        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        void UpdatePointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        bool Save();
        void DeleteCity(City cityFromRepo);
        IEnumerable<Language> GetLanguages(int cityID);
        IEnumerable<Language> GetLanguages();
        Language GetLanguage(int languageId);
        void AddLanguage(Language finalLanguage);
        void DeleteLanguages(Language languageFromRepo);
        void UpdateLanguage(Language languageFromRepo);
        Language GetLanguageFromCity(int id, int languageId);
        bool LanguageExists(int id);
        void AddLanguageToCity(int cityId, int languageId);
        void RemoveLanguageToCity(CityLanguage cityLanguage);
        CityLanguage GetCityLanguageLink(int cityId, int languageId);
    }
}

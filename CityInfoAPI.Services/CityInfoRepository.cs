using CityInfoAPI.Data;
using CityInfoAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<City> GetCities(string orderby = "Name")
        {
            var collection = _context.Cities.
                Include(c => c.CityLanguages).ThenInclude(cl => cl.Language)
                .Include(c => c.PointsOfInterest) as IQueryable<City>; 

            if(!string.IsNullOrWhiteSpace(orderby))
            {
                switch (orderby)
                {
                    case "Lang":
                        collection.OrderBy(c => c.CityLanguages).ThenBy(c => c.Name);
                        break;
                    case "Name":
                    default:
                        collection.OrderBy(c => c.Name);
                        break;
                }
            }

            return collection.ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if(includePointsOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefault();
            }

            return _context.Cities
                .Where(c => c.Id == cityId).FirstOrDefault();
        }

        public void AddCity(City city)
        {
            _context.Cities.Add(city);
        }

        public void UpdateCity(City city)
        {
        }

        public IEnumerable<PointOfInterest> GetPointOfInterestForCity(int cityId)
        {
            return _context.PointOfInterest
                .Where(p => p.CityId == cityId).ToList();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointOfInterest
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefault();
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(c => c.Id == cityId);
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            city.PointsOfInterest.Add(pointOfInterest);
        }

        public void UpdatePointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {

        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointOfInterest.Remove(pointOfInterest);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public void DeleteCity(City cityFromRepo)
        {
            _context.Remove(cityFromRepo);
        }

        public IEnumerable<Language> GetLanguages(int cityID)
        {
            return _context.Languages.
                SelectMany(v => v.CityLanguages)
                .Where(c => c.CityId == cityID)
                .Select(c => c.Language).ToList();
        }

        public IEnumerable<Language> GetLanguages()
        {
            return _context.Languages.ToList();
        }

        public Language GetLanguage(int languageId)
        {
            return _context.Languages
                .SingleOrDefault(l => l.Id == languageId);
        }

        public void AddLanguage(Language Language)
        {
            _context.Languages.Add(Language);
        }

        public void DeleteLanguages(Language languageFromRepo)
        {
            _context.Languages.Remove(languageFromRepo);
        }

        public void UpdateLanguage(Language languageFromRepo)
        {
        }

        public Language GetLanguageFromCity(int cityId, int languageId)
        {
            return _context.Languages.
                SelectMany(v => v.CityLanguages)
                .Where(c => c.CityId == cityId)
                .Select(c => c.Language).SingleOrDefault(cl => cl.Id == languageId);
        }

        public bool LanguageExists(int id)
        {
            return _context.Languages.Any(l => l.Id == id);
        }

        public void AddLanguageToCity(int cityId, int languageId)
        {
            var link = new CityLanguage
            {
                CityId = cityId,
                LanguageId = languageId
            };

            _context.CityLanguages.Add(link);
        }

        public void RemoveLanguageToCity(CityLanguage cityLanguage)
        {
            _context.CityLanguages.Remove(cityLanguage);
        }

        public CityLanguage GetCityLanguageLink(int cityId, int languageId)
        {
            return _context.CityLanguages.SingleOrDefault(cl => cl.CityId == cl.CityId && cl.LanguageId == languageId);
        }

        public IEnumerable<City> GetCitiesFromLanguages(int languageID)
        {
            return _context.Cities.Include(x => x.CityLanguages).ThenInclude(x=>x.Language).Include(x => x.PointsOfInterest).Where(x => x.CityLanguages.Any(cl => cl.LanguageId == languageID));
        }
    }
}

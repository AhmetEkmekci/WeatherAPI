using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Provider.WeatherData;
using WTR.Business.DTO;
using WTR.DataAccess.Repository;
using WTR.Domain;
using WTR.ExceptionDomain;

namespace WTR.Business.Service
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IWeatherDataProvider _weatherDataProvider;

        public LocationService(ILocationRepository locationRepository, IWeatherDataProvider weatherDataProvider)
        {
            _locationRepository = locationRepository;
            _weatherDataProvider = weatherDataProvider;
        }

        public async Task AddLocationAsync(LocationDto addLocationRequest)
        {
            var location = new Location
            {
                Created = DateTime.Now,
                Lat = addLocationRequest.Lat,
                Lon = addLocationRequest.Lon,
                Name = addLocationRequest.Name,
            };

            await _locationRepository.AddAsync(location);
        }

        /// <summary>
        /// For test purpose.
        /// </summary>
        public async Task<int> InitLocationsAsync(string LocationNameQuery)
        {
            //await _locationRepository.DeleteAllAsync();

            var data = await _weatherDataProvider.LocationsAsync(LocationNameQuery);
            var mappedData = data.Select(x => new Location() { Name = x.Name, Created = DateTime.Now, Lat = x.Lat, Lon = x.Lon });
            await _locationRepository.AddAsync(mappedData);

            return data.Count();

        }

        /// <summary>
        /// For test purpose.
        /// </summary>
        public async Task DeleteAll()
        {
            await _locationRepository.DeleteAllAsync();
        }

        public async Task<IEnumerable<LocationDto>> GetAllLocationsAsync()
        {
            var locations = await _locationRepository.GetAllAsync();

            return locations.Select(x => new LocationDto
            {
                Id = x.Id,
                Name = x.Name,
                Lat = x.Lat,
                Lon = x.Lon,
            });

        }

        public async Task<LocationDto> GetByIdAsync(int id)
        {
            var data = await _locationRepository.GetAsync(id);
            if (data is null) throw new WeatherCustomException("Location not found.");
            return new LocationDto() 
            {
                Id = data.Id,
                Name = data.Name,
                Lat = data.Lat,
                Lon = data.Lon,
            };
        }
    }
}

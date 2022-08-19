using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Provider.WeatherData;
using WTR.Abstraction.Provider.WeatherData.Mapping;
using WTR.Business.DTO;
using WTR.Business.OpenWeatherMapProvider.Helper;
using WTR.DataAccess.Repository;
using WTR.Domain;

namespace WTR.Business.Service
{
    public class ForecastService : IForecastService
    {
        private readonly IWeatherDataProvider _weatherDataProvider;
        private readonly ILocationService _locationService;
        private readonly IMemoryCache _memoryCache;


        public ForecastService(ILocationService locationService, IWeatherDataProvider weatherDataProvider, IMemoryCache memCache)
        {
            _locationService = locationService;
            _weatherDataProvider = weatherDataProvider;
            _memoryCache = memCache;
        }

        public async Task<WeatherSummaryDto> ForecastSummary(TemperatureUnit unit, double temperature, IEnumerable<int> locations)
        {
            var tomorrowStart = new DateTimeOffset(DateTime.Now.AddDays(1).Date).ToUnixTimeSeconds();
            var tomorrowEnd = tomorrowStart + (24 * 60 * 60);
            var result = new WeatherSummaryDto() { locations = new List<int>(), };
            
            foreach (var l in locations)
            {
                var locationKey = $"location_{l}";
                if (!_memoryCache.TryGetValue(locationKey, out LocationDto location))
                {
                    location = await _locationService.GetByIdAsync(l);
                    _memoryCache.Set(locationKey, location);
                }

                var forecastKey = $"forecast_{location.Lat}_{location.Lon}";
                if (
                    !_memoryCache.TryGetValue(forecastKey, out IEnumerable<IForecastMapping> locationForecast) ||
                    !locationForecast.Any(x=>x.TimeStamp >= tomorrowStart)
                    )
                {
                    locationForecast = await _weatherDataProvider.ForecastsAsync(location.Lat, location.Lon);
                    _memoryCache.Set(forecastKey, locationForecast);
                }

                var _unit = TemperatureHelper.TemperatureConvert(temperature, unit, TemperatureUnit.Celsius);
                if (locationForecast.Any(x => x.Temp > _unit && x.TimeStamp > tomorrowStart && x.TimeStamp < tomorrowEnd))
                    result.locations.Add(location.Id);
            }

            return result;

        }

        public async Task<IEnumerable<LocationForecastDto>> LocationForecast(int id)
        {
            var result = new List<LocationForecastDto>();

                var locationKey = $"location_{id}";
                if (!_memoryCache.TryGetValue(locationKey, out LocationDto location))
                {
                    location = await _locationService.GetByIdAsync(id);
                    _memoryCache.Set(locationKey, location);
                }

                var now = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                var forecastKey = $"forecast_{location.Lat}_{location.Lon}";
                if (
                        !_memoryCache.TryGetValue(forecastKey, out IEnumerable<IForecastMapping> locationForecast) ||
                        locationForecast.FirstOrDefault()?.TimeStamp < now
                    )
                {
                    locationForecast = await _weatherDataProvider.ForecastsAsync(location.Lat, location.Lon);
                    _memoryCache.Set(forecastKey, locationForecast);
                }

            return locationForecast.Select(x => new LocationForecastDto() 
            {
                Temp = x.Temp,
                TimeStamp = x.TimeStamp
            });

        }
    }
}

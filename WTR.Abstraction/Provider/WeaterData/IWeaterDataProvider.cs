using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Provider.WeatherData.Mapping;

namespace WTR.Abstraction.Provider.WeatherData
{
    public interface IWeatherDataProvider
    {
        TemperatureUnit DefaultUnit { get; }
        public Task<IEnumerable<ILocationMapping>> LocationsAsync(string LocationNameQuery);
        public Task<IEnumerable<IForecastMapping>> ForecastsAsync(double lat, double lon);
    }
}

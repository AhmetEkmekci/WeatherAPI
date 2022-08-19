using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Provider.WeatherData;

namespace WTR.Business.OpenWeatherMapProvider
{
    public class OpenWeatherMapConfig
    {
        public int TimeoutInSeconds { get; set; }
        public string APIKey { get; set; }
        public string BaseURL { get; set; }
        
        public string GeoLocationAPISlug { get; set; }
        public string GeoLocationLimit { get; set; }
     
        public string ForecastAPISlug { get; set; }
        public TemperatureUnit ForecastDefaultUnit { get; set; }
        public Dictionary<TemperatureUnit, string> UnitMapping { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Provider.WeatherData.Mapping;

namespace WTR.Business.OpenWeatherMapProvider.Mapping
{
    public class LocationModel : ILocationMapping
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}

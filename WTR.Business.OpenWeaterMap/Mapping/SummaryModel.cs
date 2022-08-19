using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Provider.WeatherData.Mapping;

namespace WTR.Business.OpenWeatherMapProvider.Mapping
{
    public class SummaryModel : IForecastMapping
    {
        public long TimeStamp { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Temp { get; set; }
    }
}

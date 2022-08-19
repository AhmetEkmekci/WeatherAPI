using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Provider.WeatherData.Mapping;

namespace WTR.Business.DTO
{
    public class LocationForecastDto 
    {
        internal DateTime Date { get { return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(TimeStamp).ToLocalTime(); } }
        public long TimeStamp { get; set; }
        public double Temp { get; set; }
    }
}

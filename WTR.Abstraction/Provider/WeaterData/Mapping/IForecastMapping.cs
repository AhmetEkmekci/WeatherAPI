using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTR.Abstraction.Provider.WeatherData.Mapping
{
    public interface IForecastMapping
    {
        long TimeStamp { get; set; }
        double Temp { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Provider.WeatherData;

namespace WTR.Business.OpenWeatherMapProvider.Helper
{
    internal static class TemperatureHelper
    {
        internal static Func<double, TemperatureUnit, TemperatureUnit, double> TemperatureConvert = (double value, TemperatureUnit source, TemperatureUnit target) =>
        {
            // F = (C * 9) / 5 + 32;
            // C = (F - 32) * 5 / 9;
            if (source == target)
                return value;
            else if (source == TemperatureUnit.Celsius && target == TemperatureUnit.Fahrenheit)
                return (value * 9) / 5 + 32;
            else //if (source == TemperatureUnit.Fahrenheit && target == TemperatureUnit.Celsius)
                return (value - 32) * 5 / 9;
        };
    }
}

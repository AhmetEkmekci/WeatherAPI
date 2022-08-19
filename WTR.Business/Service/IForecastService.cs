using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Provider.WeatherData;
using WTR.Business.DTO;

namespace WTR.Business.Service
{
    public interface IForecastService
    {
        Task<WeatherSummaryDto> ForecastSummary(TemperatureUnit unit, double temperature, IEnumerable<int> locations);
        Task<IEnumerable<LocationForecastDto>> LocationForecast(int locations);
    }
}

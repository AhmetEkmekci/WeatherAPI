using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Provider.WeaterData;

namespace WTR.Business.OpenWeatherMapProvider
{
    public class OpenWeatherMapProviderException : WeatherProviderExceptionBase
    {
        public OpenWeatherMapProviderException(string Message) : base(Message)
        {

        }
    }
}

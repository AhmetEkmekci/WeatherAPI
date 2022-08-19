using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.ExceptionDomain;

namespace WTR.Abstraction.Provider.WeaterData
{
    public abstract class WeatherProviderExceptionBase : WeatherCustomException
    {
        public WeatherProviderExceptionBase(string Message) : base(Message)
        {

        }
    }
}

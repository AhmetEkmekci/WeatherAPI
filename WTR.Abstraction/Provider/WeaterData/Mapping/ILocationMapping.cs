using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTR.Abstraction.Provider.WeatherData.Mapping
{
    public interface ILocationMapping
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}

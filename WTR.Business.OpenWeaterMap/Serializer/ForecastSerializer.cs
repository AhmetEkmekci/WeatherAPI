using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTR.Business.OpenWeatherMapProvider.Serializer
{
    public class ForecastSerializer
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public List<_list> list { get; set; }
        public class _list
        {
            public long dt { get; set; }
            public class _main
            {
                public double temp { get; set; }
            }
            public _main main { get; set; }
        }
    }
}

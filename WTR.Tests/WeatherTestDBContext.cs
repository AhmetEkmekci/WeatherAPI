using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Entity;
using WTR.DataAccess.Data;
using WTR.Domain;

namespace WTR.Tests
{
    internal class WeatherTestDBContext : WeatherDBContext
    {
        public WeatherTestDBContext(DbContextOptions<WeatherDBContext> options) : base(options)
        {
        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            var data = 
@"[{""id"":44,""name"":""Copenhagen"",""lat"":55.6867243,""lon"":12.5700724},{""id"":45,""name"":""Istanbul"",""lat"":41.0096334,""lon"":28.9651646},{""id"":46,""name"":""London"",""lat"":51.5073219,""lon"":-0.1276474},{""id"":47,""name"":""City of London"",""lat"":51.5156177,""lon"":-0.0919983},{""id"":48,""name"":""Tokyo"",""lat"":35.6828387,""lon"":139.7594549},{""id"":49,""name"":""Amsterdam"",""lat"":52.3727598,""lon"":4.8936041},{""id"":50,""name"":""Amsterdam"",""lat"":52.37454030000001,""lon"":4.897975505617977}]";
            base.OnModelCreating(modelBuilder);
            seedDataWithJsonFile<Location>(modelBuilder, data);
        }

        void seedDataWithJsonFile<T>(ModelBuilder modelBuilder, string json) where T : class, IEntity
        {
            var data = JsonConvert.DeserializeObject<List<T>>(json);
            modelBuilder.Entity<T>().HasData(data);
        }
    }
}

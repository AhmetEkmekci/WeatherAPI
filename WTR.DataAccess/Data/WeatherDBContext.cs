using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Context;
using WTR.Domain;

namespace WTR.DataAccess.Data
{
    public class WeatherDBContext : DbContext, IWeaeterContext
    {
        public WeatherDBContext(DbContextOptions<WeatherDBContext> options) : base(options)
        {

        }

        public DbSet<Location> Locations { get; set; }
    }
}

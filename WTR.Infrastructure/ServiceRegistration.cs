using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WTR.DataAccess.Data;
using WTR.DataAccess.Repository;
using WTR.Infrastructure.Services.Log;
using WTR.Abstraction.Provider.WeatherData;
using WTR.Business.OpenWeatherMapProvider;
using WTR.Business.Service;

namespace WTR.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, WebApplicationBuilder builder, IConfiguration configuration)
        {
            #region Log
            services.AddSingleton(typeof(ILogService), typeof(LogService));
            #endregion

            #region Cache
            builder.Services.AddMemoryCache();
            #endregion



            var connectionString = builder.Configuration.GetConnectionString("db");
            var host = builder.Configuration.GetValue<string>("DefaultSqlHost");
            connectionString = connectionString.Replace("[HOST]", host);

            builder.Services.AddDbContext<WeatherDBContext>(conf => conf.UseSqlServer(connectionString, db => db.MigrationsAssembly("WTR.DataAccess")), ServiceLifetime.Transient);
            builder.Services.AddScoped<ILocationRepository, EFLocationRepository>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IForecastService, ForecastService>();
            

            #region OpenWeatherMap
            builder.Services.AddSingleton(_ => new OpenWeatherMapConfig() {
                APIKey = builder.Configuration.GetSection("openweathermap").GetValue<string>("APIKey"),
                BaseURL = builder.Configuration.GetSection("openweathermap").GetValue<string>("BaseURL"),
                TimeoutInSeconds = int.Parse(builder.Configuration.GetSection("openweathermap").GetValue<string>("TimeoutInSeconds")),
                ForecastAPISlug = builder.Configuration.GetSection("openweathermap").GetSection("ForecastAPI").GetValue<string>("APISlug"),
                ForecastDefaultUnit = builder.Configuration.GetSection("openweathermap").GetSection("ForecastAPI").GetValue<TemperatureUnit>("DefaultUnit"),
                GeoLocationAPISlug = builder.Configuration.GetSection("openweathermap").GetSection("GeoLocationAPI").GetValue<string>("APISlug"),
                GeoLocationLimit = builder.Configuration.GetSection("openweathermap").GetSection("GeoLocationAPI").GetValue<string>("Limit"),
                UnitMapping = Enum.GetValues<Abstraction.Provider.WeatherData.TemperatureUnit>()
                    .ToDictionary(x=>x, x=> builder.Configuration.GetSection("openweathermap").GetSection("ForecastAPI").GetSection("UnitPairs").GetValue<string>(x.ToString())),
            });
            builder.Services.AddScoped<IWeatherDataProvider, OpenWeatherMapDataProvider>();

            #endregion
            builder.Services.AddResponseCaching();
        }
    }
}
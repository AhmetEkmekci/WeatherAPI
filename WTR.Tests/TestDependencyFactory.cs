using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WTR.Abstraction.Provider.WeatherData;
using WTR.Business.Service;
using WTR.DataAccess.Data;
using WTR.DataAccess.Repository;

namespace WTR.Tests
{
    internal static class TestDependencyFactory
    {
        internal static WeatherDBContext ConfigureDBContext()
        {
            var options = new DbContextOptionsBuilder<WeatherDBContext>().UseInMemoryDatabase("inMemory").Options;

            var testDBContext = new WeatherTestDBContext(options);
            var db = new WeatherDBContext(options);
            db.Database.EnsureCreated();

            return testDBContext;
        }

        internal static ILocationService ConfigureLocationService(string LocationNameQuery)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IWeatherDataProvider>(TestDependencyFactory.ConfigureMockWeatherDataProvider(LocationNameQuery));
            services.AddSingleton<ILocationRepository>(TestDependencyFactory.ConfigureMockLocationRepository());
            services.AddSingleton<IForecastService>(TestDependencyFactory.ConfigureMockForecastService());
            services.AddTransient<ILocationService, LocationService>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<ILocationService>();
        }


        internal static Domain.Location MockLocation = new Domain.Location() { Id = 1, Created = DateTime.Now, Lat = 1, Lon = 1, Name = "MockLocation" };
        internal static WTR.Business.DTO.LocationDto MockLocationDto = new WTR.Business.DTO.LocationDto() { Id = MockLocation.Id, Lat = MockLocation.Lat, Lon = MockLocation.Lon, Name = MockLocation.Name };
        internal static WTR.Business.DTO.LocationForecastDto MockLocationForecastDto = new WTR.Business.DTO.LocationForecastDto() { Temp = 10, TimeStamp = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() };
        internal static WTR.Business.OpenWeatherMapProvider.Mapping.LocationModel MockLocationModel = new WTR.Business.OpenWeatherMapProvider.Mapping.LocationModel() { Lat = MockLocation.Lat, Lon = MockLocation.Lon, Name = MockLocation.Name };
        internal static WTR.Business.OpenWeatherMapProvider.Mapping.SummaryModel MockSummaryModel = new WTR.Business.OpenWeatherMapProvider.Mapping.SummaryModel() { Lat = MockLocation.Lat, Lon = MockLocation.Lon, Temp = 10, TimeStamp = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() };
        internal static ILocationRepository ConfigureMockLocationRepository()
        {
            var mock = new Mock<ILocationRepository>();
            mock.Setup(x => x.AddAsync(MockLocation));
            mock.Setup(x => x.UpdateAsync(MockLocation));
            mock.Setup(x => x.DeleteAsync(MockLocation.Id));
            mock.Setup(x => x.DeleteAllAsync());
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Domain.Location>() { MockLocation });
            mock.Setup(x => x.GetAsync(MockLocation.Id)).ReturnsAsync(MockLocation);

            return mock.Object;
        }

        internal static IForecastService ConfigureMockForecastService()
        {
            var mock = new Mock<IForecastService>();

            mock.Setup(x => x.ForecastSummary(Abstraction.Provider.WeatherData.TemperatureUnit.Celsius, 10, new List<int>() { MockLocation.Id }))
                .ReturnsAsync(new WTR.Business.DTO.WeatherSummaryDto() { locations = new List<int>() { MockLocation.Id } });

            mock.Setup(x => x.LocationForecast(MockLocation.Id))
                .ReturnsAsync(new List<WTR.Business.DTO.LocationForecastDto>() { });


            return mock.Object;
        }

        internal static IWeatherDataProvider ConfigureMockWeatherDataProvider(string LocationNameQuery)
        {
            var mock = new Mock<IWeatherDataProvider>();

            mock.Setup(x => x.DefaultUnit).Returns(TemperatureUnit.Celsius);

            mock.Setup(x => x.ForecastsAsync(MockLocation.Lat, MockLocation.Lon))
                .ReturnsAsync(new List<WTR.Business.OpenWeatherMapProvider.Mapping.SummaryModel>() { MockSummaryModel });

            mock.Setup(x => x.LocationsAsync(LocationNameQuery))
                .ReturnsAsync(new List<WTR.Business.OpenWeatherMapProvider.Mapping.LocationModel>() { MockLocationModel });


            return mock.Object;
        }


    }
}
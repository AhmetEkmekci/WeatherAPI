using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Provider.WeatherData;
using WTR.Business.Service;
using WTR.DataAccess.Repository;

namespace WTR.Tests.Business
{
    [TestClass]
    public class LocationServiceUnitTests
    {

        public LocationServiceUnitTests()
        {
            
        }

        [TestMethod]
        public void AddLocationAsync_ShouldAdd()
        {
            var locationService = TestDependencyFactory.ConfigureLocationService(null);
            locationService.AddLocationAsync(TestDependencyFactory.MockLocationDto);
        }

        [TestMethod]
        [DataRow("Copenhagen,Capital Region of Denmark,DK")]
        [DataRow("Istanbul,TR")]
        public void InitLocationsAsync_ShouldInit(string LocationNameQuery)
        {
            var locationService = TestDependencyFactory.ConfigureLocationService(LocationNameQuery);
            var result = locationService.InitLocationsAsync(LocationNameQuery).Result;

            Assert.IsNotNull(result);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void GetAllLocationsAsync_ShouldReturnLocations()
        {
            var locationService = TestDependencyFactory.ConfigureLocationService(null);
            var result = locationService.GetAllLocationsAsync().Result;
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(x=> x.Id == TestDependencyFactory.MockLocationDto.Id));
        }

        [TestMethod]
        public void DeleteAll_ShouldDelete()
        {
            var locationService = TestDependencyFactory.ConfigureLocationService(null);
            var result = locationService.DeleteAll();

            Assert.IsNotNull(result);
        }

    }
}

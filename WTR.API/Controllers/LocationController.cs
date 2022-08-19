using Microsoft.AspNetCore.Mvc;
using WTR.Business.DTO;
using WTR.Business.Service;

namespace WTR.API.Controllers
{
    [ApiController]
    [Route("weather/[controller]")]
    public class LocationsController : ControllerBase
    {
       

        private readonly ILogger<LocationsController> _logger;
        private readonly ILocationService _locationService;
        private readonly IForecastService _forecastService;

        public LocationsController(ILogger<LocationsController> logger, ILocationService locationService, IForecastService forecastService)
        {
            _logger = logger;
            _locationService = locationService;
            _forecastService = forecastService;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<LocationForecastDto>> Get([FromRoute]int id)
        {
            return await _forecastService.LocationForecast(id);
        }

        [HttpGet(Name = "GetAllLocations")]
        public async Task<IEnumerable<LocationDto>> GetAll()
        {
            return await _locationService.GetAllLocationsAsync();
        }

#if DEBUG
        /// <summary>
        /// For test purpose.
        /// </summary>
        [HttpPost(Name = "InitLocations")]
        public async Task<int> Put(string LocationNameQuery = "Copenhagen,Capital Region of Denmark,DK")
        {
            var data = await _locationService.InitLocationsAsync(LocationNameQuery);
            return data;
        }

        /// <summary>
        /// For test purpose.
        /// </summary>
        [HttpDelete(Name = "DeleteAllLocations")]
        public async Task<ActionResult> DeleteAll()
        {
            await _locationService.DeleteAll();
            return Ok(true);
        }
#endif

    }
}
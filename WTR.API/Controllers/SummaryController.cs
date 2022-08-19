using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WTR.Abstraction.Provider.WeatherData;
using WTR.Business.DTO;
using WTR.Business.Service;
using WTR.Infrastructure.Services.CommaSeparated;

namespace WTR.API.Controllers
{
    [ApiController]
    [Route("weather/[controller]")]
    public class SummaryController : ControllerBase
    {


        private readonly ILogger<SummaryController> _logger;
        private readonly IForecastService _forecastService;

        public SummaryController(ILogger<SummaryController> logger, IForecastService forecastService)
        {
            _logger = logger;
            _forecastService = forecastService;
        }



        [HttpGet(Name = "GetSummary")]
        public async Task<WeatherSummaryDto> Get(TemperatureUnit unit, [Required] int temperature, [Required] CommaSeparatedIntListModelBinder locations)
        {
            return await _forecastService.ForecastSummary(unit, temperature, locations);
        }
    }
}
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using WTR.Abstraction.Provider.WeatherData;
using WTR.Abstraction.Provider.WeatherData.Mapping;
using WTR.Business.OpenWeatherMapProvider.Mapping;
using WTR.Business.OpenWeatherMapProvider.Serializer;

namespace WTR.Business.OpenWeatherMapProvider
{
    public class OpenWeatherMapDataProvider : IWeatherDataProvider
    {
        readonly OpenWeatherMapConfig _config;
        public OpenWeatherMapDataProvider(OpenWeatherMapConfig config)
        {
            if (config is null)
                throw new OpenWeatherMapProviderException("config is null.");
            this._config = config;
        }

        public TemperatureUnit DefaultUnit { get { return _config.ForecastDefaultUnit; } }

        public async Task<IEnumerable<ILocationMapping>> LocationsAsync(string LocationNameQuery)
        {
            try
            {
                HttpClient _httpClient = new HttpClient();
                var query = new Dictionary<string, string>()
            {
                { "appid", _config.APIKey },
                { "limit", _config.GeoLocationLimit },
                { "q", LocationNameQuery }
            };

                var url = QueryHelpers.AddQueryString($"{_config.BaseURL}/{_config.GeoLocationAPISlug}", query);
                _httpClient.BaseAddress = new Uri(url);
                _httpClient.Timeout = TimeSpan.FromSeconds(_config.TimeoutInSeconds);
                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Get;

                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var locations = new List<LocationModel>();
                if (response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    locations = JsonSerializer.Deserialize<List<LocationModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                return locations;
            }
            catch (TimeoutException ex)
            {
                throw new OpenWeatherMapProviderException("Connection timeout in Open weather api entegration.");
            }
            catch (HttpRequestException ex)
            {
                throw new OpenWeatherMapProviderException($"{ex.StatusCode} http exception in Open weather api entegration.");
            }
            catch (Exception ex)
            {
                throw new OpenWeatherMapProviderException("Unexpected error occurred in Open weather api entegration.");
            }

        }
        public async Task<IEnumerable<IForecastMapping>> ForecastsAsync(double lat, double lon)
        {
            try
            {
                HttpClient _httpClient = new HttpClient();
                var query = new Dictionary<string, string>()
            {
                { "appid", _config.APIKey },
                { "lat", lat.ToString() },
                { "lon", lon.ToString() },
                { "units", _config.UnitMapping[_config.ForecastDefaultUnit]}
            };

                var url = QueryHelpers.AddQueryString($"{_config.BaseURL}/{_config.ForecastAPISlug}", query);
                _httpClient.BaseAddress = new Uri(url);
                _httpClient.Timeout = TimeSpan.FromSeconds(_config.TimeoutInSeconds);
                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Get;

                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var forecast = new List<SummaryModel>();
                if (response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    var _forecast = JsonSerializer.Deserialize<ForecastSerializer>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });
                    forecast = _forecast.list.Select(x => new SummaryModel()
                    {
                        Lat = lat,
                        Lon = lon,
                        Temp = x.main.temp,
                        TimeStamp = x.dt
                    }).ToList();
                }
                return forecast;
            }
            catch (TimeoutException ex)
            {
                throw new OpenWeatherMapProviderException("Connection timeout in Open weather api entegration.");
            }
            catch (HttpRequestException ex)
            {
                if(ex.StatusCode.HasValue)
                    throw new OpenWeatherMapProviderException($"{(int)ex.StatusCode}({ex.StatusCode}) http exception in Open weather api entegration.");
                else
                    throw new OpenWeatherMapProviderException($"http request exception in Open weather api entegration.");
            }
            catch (Exception ex)
            {
                throw new OpenWeatherMapProviderException("Unexpected error occurred in Open weather api entegration.");
            }
        }
    }
}
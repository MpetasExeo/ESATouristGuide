using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;

using Newtonsoft.Json;

using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

using ESATouristGuide.Models.cmportal.Models;
using ESATouristGuide.Resources;

namespace ESATouristGuide.Services
{
    public class WeatherService : IWeatherService
    {
        HttpClient _httpClient;

        public async Task<Temperatures> GetCurrentWeatherAsync( Position position , CancellationToken cancellationToken )
        {
            _httpClient = new HttpClient();
            Temperatures Temperature = new Temperatures { WeatherImage = new Uri("data:image/png;base64,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") , Temperature = AppResources.TimeSpanError }; ;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {             //Rootobject rawWeather = new Rootobject();
                _httpClient.BaseAddress = new Uri("https://api.openweathermap.org");

                CultureInfo culture = new CultureInfo("el");

                HttpResponseMessage response = await _httpClient.GetAsync($"/data/2.5/weather?lat={position.Latitude.ToString(culture)}&lon={position.Longitude.ToString(culture)}&appid={"c7f584767245ebb4f4b36ec0426dda9f"}&units=metric").ConfigureAwait(true);
                _ = response.EnsureSuccessStatusCode();

                string stringResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                CurrentWeatherResponse currentWeather = JsonConvert.DeserializeObject<CurrentWeatherResponse>(stringResult);

                string icon = currentWeather.Weather[0].Icon;
                string imageString = $"https://openweathermap.org/img/wn/{icon}@4x.png";

                Temperature.WeatherImage = new Uri(imageString);

                int temp = (int)currentWeather.Main.Temp;
                Temperature.Temperature = $"{temp}°C";

                return Temperature;
            }

            return Temperature;
        }

    }
}

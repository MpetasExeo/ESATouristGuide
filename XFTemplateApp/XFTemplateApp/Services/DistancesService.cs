using Newtonsoft.Json;

using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

using XFTemplateApp.Interfaces;
using XFTemplateApp.Models;
using XFTemplateApp.Resources;

namespace XFTemplateApp.Services
{
    public class DistancesService : IDistancesService
    {

        HttpClient _httpClient;

        public async Task<Distances> GetDistancesFromUserAsync( Position PlacePosition , Position UserPosition )
        {
            // default values
            Distances distances = new Distances { DrivingDuration = AppResources.TimeSpanError , Distance = AppResources.TimeSpanError , WalkingDuration = AppResources.TimeSpanError };

            // αν δεν έχει ίντερνετ || UserPosition == dummyPosition ==> return default values
            if (Connectivity.NetworkAccess != NetworkAccess.Internet || UserPosition == new Position(40.5000001 , 22.9500001))
            {
                return distances;
            }

            // απόσταση πάντα >= 0 ==> -1 για έλεγχο αν βρέθηκε απόσταση
            int dist = -1;

            string mode = "driving";

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            GoogleApiResponse result;

            string parameters = string.Format("destinations={0},{1}&origins={2},{3}&mode={4}&key={5}" ,
                                    PlacePosition.Latitude.ToString(culture) ,
                                    PlacePosition.Longitude.ToString(culture) ,
                                    UserPosition.Latitude.ToString(culture) ,
                                    UserPosition.Longitude.ToString(culture) ,
                                    mode ,
                                    Constants.GoogleApiKey);

            _httpClient = new HttpClient()
            {
                /* Διαμορφώνω το request*/
                BaseAddress = new Uri("https://maps.googleapis.com")
            };
            HttpResponseMessage response = await _httpClient.GetAsync($"/maps/api/distancematrix/json?{parameters}")
                .ConfigureAwait(true);
            _ = response.EnsureSuccessStatusCode();

            string stringResult = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            result = JsonConvert.DeserializeObject<GoogleApiResponse>(stringResult);

            TimeSpan drivingDuration;
            if (result.rows[0].elements[0].duration != null)
            {
                dist = result.rows[0].elements[0].distance.value / 1000;
            }

            drivingDuration = result.rows[0]
                .elements[0].duration != null
                ? TimeSpan.FromSeconds(result.rows[0].elements[0].duration.value)
                : TimeSpan.FromSeconds(-1);

            mode = "walking";

            parameters = $"destinations={PlacePosition.Latitude.ToString(culture)},{PlacePosition.Longitude.ToString(culture)}&origins={UserPosition.Latitude.ToString(culture)},{UserPosition.Longitude.ToString(culture)}&mode={mode}&key={Constants.GoogleApiKey}";

            response = await _httpClient.GetAsync($"/maps/api/distancematrix/json?{parameters}")
                                        .ConfigureAwait(true);

            _ = response.EnsureSuccessStatusCode();

            stringResult = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            result = JsonConvert.DeserializeObject<GoogleApiResponse>(stringResult);

            TimeSpan walkingDuration = result.rows[0].elements[0].duration != null
                ? TimeSpan.FromSeconds(result.rows[0].elements[0].duration.value)
                : TimeSpan.FromSeconds(-1);


            DurationFormatting(distances , drivingDuration , walkingDuration , dist);

            return distances;

        }

        public void DurationFormatting( Distances distances , TimeSpan driving , TimeSpan walking , float distance )
        {
            // formatting durations
            distances.WalkingDuration = TimeSpanFormatting(walking.Days , walking.Hours , walking.Minutes);
            distances.DrivingDuration = TimeSpanFormatting(driving.Days , driving.Hours , driving.Minutes);

            // formatting distance αν βρέθηκε απόσταση
            if (distance > -1)
            {
                distances.Distance = $"{distance.ToString()}{AppResources.KilometersShort}";
                return;
            }

            //αν δεν βρέθηκε απόσταση ==> δείξε "NA"
            distances.Distance = AppResources.TimeSpanError;
        }

        public string TimeSpanFormatting( int days , int hours , int minutes )
        {
            string Format = AppResources.NaN;

            // δεν βρέθηκε απόσταση
            //if (timeSpan.Seconds == -1) return Format = string.Format("{0}", AppResources.TimeSpanError);

            // αν απόσταση έχει μέρες
            if (days > 0)
            {
                return $"{( days * 24 ) + hours}{AppResources.Hours} {minutes}{AppResources.Minutes}";
            }

            // αν απόσταση έχει ώρες
            if (hours > 0)
            {
                return $"{hours}{AppResources.Hours} {minutes}{AppResources.Minutes}";
            }

            // αν απόσταση έχει λεπτά 
            return minutes >= 0 ? $"{minutes}{AppResources.Minutes}" : Format;
        }

    }
}

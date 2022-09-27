using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;
using ESATouristGuide.Resources;

using Newtonsoft.Json;

using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

using Xamarin.Essentials;

namespace ESATouristGuide.Services
{
    public class DistancesService : IDistancesService
    {
        private HttpClient _httpClient;

        public async Task<Distances> GetDistancesFromUserAsync(Location PlaceLocation , Location UserPosition)
        {
            // default values
            Distances distances = new Distances
            {
                DrivingDuration = AppResources.TimeSpanError ,
                Distance = AppResources.TimeSpanError ,
                WalkingDuration = AppResources.TimeSpanError
            };

            // αν δεν έχει ίντερνετ || UserPosition == dummyPosition ==> return default values
            if (Connectivity.NetworkAccess != NetworkAccess.Internet ||
                UserPosition == new Location(40.5000001 , 22.9500001))
            {
                return distances;
            }

            // απόσταση πάντα >= 0 ==> -1 για έλεγχο αν βρέθηκε απόσταση
            var dist = -1;

            var mode = "driving";

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            GoogleApiResponse result;

            var parameters = string.Format(
                "destinations={0},{1}&origins={2},{3}&mode={4}&key={5}" ,
                PlaceLocation.Latitude.ToString(culture) ,
                PlaceLocation.Longitude.ToString(culture) ,
                UserPosition.Latitude.ToString(culture) ,
                UserPosition.Longitude.ToString(culture) ,
                mode ,
                Constants.GoogleApiKey);

            _httpClient = new HttpClient()
            {
                /* Διαμορφώνω το request*/
                BaseAddress = new Uri(Constants.GoogleMapsBaseAddress)
            };
            var response = await _httpClient.GetAsync($"/maps/api/distancematrix/json?{parameters}")
                .ConfigureAwait(true);
            _ = response.EnsureSuccessStatusCode();

            var stringResult = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            result = JsonConvert.DeserializeObject<GoogleApiResponse>(stringResult);

            TimeSpan drivingDuration;
            if (result.rows[0].elements[0].duration != null)
            {
                dist = result.rows[0].elements[0].distance.value / 1000;
            }

            drivingDuration = result.rows[0]
                .elements[0].duration !=
                    null
                ? TimeSpan.FromSeconds(result.rows[0].elements[0].duration.value)
                : TimeSpan.FromSeconds(-1);

            mode = "walking";

            parameters = $"destinations={PlaceLocation.Latitude.ToString(culture)},{PlaceLocation.Longitude.ToString(culture)}&origins={UserPosition.Latitude.ToString(culture)},{UserPosition.Longitude.ToString(culture)}&mode={mode}&key={Constants.GoogleApiKey}";

            response = await _httpClient.GetAsync($"/maps/api/distancematrix/json?{parameters}").ConfigureAwait(true);

            _ = response.EnsureSuccessStatusCode();

            stringResult = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            result = JsonConvert.DeserializeObject<GoogleApiResponse>(stringResult);

            var walkingDuration = result.rows[0].elements[0].duration != null
                ? TimeSpan.FromSeconds(result.rows[0].elements[0].duration.value)
                : TimeSpan.FromSeconds(-1);


            DurationFormatting(distances , drivingDuration , walkingDuration , dist);

            return distances;
        }


        /// <summary>
        /// Μορφοποιεί τα strings των αποστάσεων.<typeparamref name="distances"/> <see cref="distances">Link text</see>
        /// </summary>
        /// <param name="distances"></param>
        /// <param name="driving">χρόνος με αμάξι</param>
        /// <param name="walking">χρόνος με τα πόδια</param>
        /// <param name="distance">μήκος συντομότερης διαδρομής</param>
        public void DurationFormatting(Distances distances , TimeSpan driving , TimeSpan walking , float distance)
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


        public string TimeSpanFormatting(int days , int hours , int minutes)
        {
            var Format = AppResources.NaN;

            // δεν βρέθηκε απόσταση
            //if (timeSpan.Seconds == -1) return Format = string.Format("{0}", AppResources.TimeSpanError);

            // αν απόσταση έχει μέρες
            if (days > 0)
            {
                return $"{(days * 24) + hours}{AppResources.Hours} {minutes}{AppResources.Minutes}";
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

using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;
using ESATouristGuide.Views;

using MvvmHelpers;

using Newtonsoft.Json;

using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ESATouristGuide.Services
{
    public class GreekCitiesService : IGreekCitiesService
    {
        /// <summary>
        /// Returns the deserialized list of cities from the .json file.
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableRangeCollection<POI>> GetGreekCities()
        {
            ObservableRangeCollection<POI> greekCities = new ObservableRangeCollection<POI>();
            var jsonFolder = "Json";
            var jsonFileName = "greekcities.json";
            var assembly = typeof(GoogleMapsPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFolder}.{jsonFileName}");

            stream.Position = 0;

            using (StreamReader reader = new StreamReader(stream))
            {
                var jsonString = await reader.ReadToEndAsync().ConfigureAwait(true);
                greekCities = JsonConvert.DeserializeObject<ObservableRangeCollection<POI>>(jsonString);
            }

            return greekCities;
        }
    }
}

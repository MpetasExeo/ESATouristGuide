using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;

using MvvmHelpers;

using Newtonsoft.Json;

using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using ESATouristGuide.Views;

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
            string jsonFolder = "Json";
            string jsonFileName = "greekcities.json";
            Assembly assembly = typeof(GoogleMapsPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFolder}.{jsonFileName}");

            stream.Position = 0;

            using (StreamReader reader = new StreamReader(stream))
            {
                string jsonString = await reader.ReadToEndAsync().ConfigureAwait(true);
                greekCities = JsonConvert.DeserializeObject<ObservableRangeCollection<POI>>(jsonString);
            }

            return greekCities;
        }
    }
}

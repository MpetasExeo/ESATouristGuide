using MvvmHelpers;

using Newtonsoft.Json;

using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using XFTemplateApp.Interfaces;
using XFTemplateApp.Models;
using XFTemplateApp.Views;

namespace XFTemplateApp.Services
{
    public class GreekCitiesService : IGreekCitiesService
    {
        /// <summary>
        /// Returns the deserialized list of cities from the .json file.
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableRangeCollection<City>> GetGreekCities()
        {
            ObservableRangeCollection<City> greekCities = new ObservableRangeCollection<City>();
            string jsonFolder = "Json";
            string jsonFileName = "greekcities.json";
            Assembly assembly = typeof(GoogleMapsPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFolder}.{jsonFileName}");

            stream.Position = 0;

            using (StreamReader reader = new System.IO.StreamReader(stream))
            {
                string jsonString = await reader.ReadToEndAsync().ConfigureAwait(true);
                greekCities = JsonConvert.DeserializeObject<ObservableRangeCollection<City>>(jsonString);
            }

            return greekCities;
        }
    }
}

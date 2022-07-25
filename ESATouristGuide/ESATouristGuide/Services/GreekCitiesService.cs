using ESATouristGuide.Helpers;
using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;
using ESATouristGuide.Views;

using MvvmHelpers;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

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


        public async Task<List<POI>> GetPagedListItem(
            int tabId ,
            int[] category ,
            int? page ,
            int pageSize = 10 ,
            int window = 5 ,
            bool ascending = true)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://esa.exeo.site");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
                List<string> qs =
new List<string> { $"asceding={System.Net.WebUtility.UrlEncode(ascending.ToString())}" };

                for (int i = 0; i < category.Length; i++)
                {
                    qs.Add($"category={category[i]}");
                }
                if (page != null)
                {
                    qs.Add($"page={page}");
                }

                //string sqs = string.Join("&" , qs);

                string lang = Settings.TwoLetterLocaleCode[Settings.Language];

                HttpResponseMessage response =
client.GetAsync(string.Format("api/Contents/paged")).Result;

                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert
                                          .DeserializeObject<List<POI>>( await response.Content.ReadAsStringAsync());
                }
            }
            return null;
        }

        public POI Get(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://dmoapi.visit-centralmacedonia.gr");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string lang = Settings.TwoLetterLocaleCode[Settings.Language];

                    HttpResponseMessage response =
client.GetAsync(string.Format("api/Contents/{0}/{1}" , lang , id)).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return Newtonsoft.Json.JsonConvert
                                              .DeserializeObject<POI>(
                                                  response.Content.ReadAsStringAsync().Result);
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null; //new ElementDetails { Related = new List<ElementSlim>(), Images = new List<ResourceSlim>(), Videos = new List<ResourceSlim>(), TabId = -1, Links = new List<ResourceSlim>() };
        }
    }

}

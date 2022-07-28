using ESATouristGuide.Helpers;
using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace ESATouristGuide.Services
{
    public class ContentService : IContentService
    {
        public PagedList<POISlim> GetPagedListItem(int tabId , int[] category , int? page , int pageSize = 10 , int window = 5 , bool ascending = true)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://esa.exeo.site/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
                List<string> qs = new List<string>
                {
                    $"asceding={System.Net.WebUtility.UrlEncode(ascending.ToString())}"
                };

                for (int i = 0; i < category.Length; i++)
                {
                    qs.Add($"category={category[i]}");
                }

                if (page != null)
                {
                    qs.Add($"page={page}");
                }

                string sqs = string.Join("&" , qs);

                string lang = Settings.TwoLetterLocaleCode[Settings.Language];

                HttpResponseMessage response = client.GetAsync(string.Format("api/Contents/paged/?{0}" ,  sqs)).Result;

                if (response.IsSuccessStatusCode)
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<PagedList<POISlim>>(response.Content.ReadAsStringAsync().Result);
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
                    client.BaseAddress = new Uri("https://esa.exeo.site/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    string lang = Settings.TwoLetterLocaleCode[Settings.Language];

                    HttpResponseMessage response = client.GetAsync(string.Format("api/Contents/el/{0}" , id)).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<POI>(response.Content.ReadAsStringAsync().Result);
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null; //new ElementDetails { Related = new List<ElementSlim>(), Images = new List<ResourceSlim>(), Videos = new List<ResourceSlim>(), TabId = -1, Links = new List<ResourceSlim>() };
        }

        //public List<ARItem> GetArItems(Position userLocation , double distance)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            client.BaseAddress = new Uri("https://dmoapi.visit-centralmacedonia.gr");
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(
        //                new MediaTypeWithQualityHeaderValue("application/json"));

        //            string lang = Settings.TwoLetterLocaleCode[Settings.Language];

        //            var culture = new CultureInfo("en-US");

        //            var str = string.Format("api/Pois/ar/{0}/{1}/{2}/{3}" , lang , userLocation.Latitude.ToString(culture) , userLocation.Longitude.ToString(culture) , distance.ToString(culture));

        //            HttpResponseMessage response = client.GetAsync(str).Result;

        //            if (response.IsSuccessStatusCode)
        //            {
        //                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ARItem>>(response.Content.ReadAsStringAsync().Result);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            var st = ex.Message;
        //            return null;
        //        }
        //    }

        //    return null; //new ElementDetails { Related = new List<ElementSlim>(), Images = new List<ResourceSlim>(), Videos = new List<ResourceSlim>(), TabId = -1, Links = new List<ResourceSlim>() };
        //}

    }
}

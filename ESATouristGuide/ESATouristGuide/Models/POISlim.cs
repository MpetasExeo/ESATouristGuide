using ESATouristGuide.Helpers;
using ESATouristGuide.Interfaces;
using ESATouristGuide.Services;
using ESATouristGuide.ViewModels;
using ESATouristGuide.Views;

using Newtonsoft.Json;

using System;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace ESATouristGuide.Models
{
    public class POISlim
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("ImageUrl")]
        public Uri ImageUrl { get; set; }

        [JsonProperty(propertyName: "Content")]
        public string Content { get; set; }

        [JsonProperty("Latitude")]
        public double? Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double? Longitude { get; set; }
        public double Distance => CalculateDistanceFromUser();
        public int CategoryId => GetTags();
        public bool ShowDistance
        {
            get
            {
                if ((Distance < 1500) && (Distance > 0.1))
                {
                    return true;
                }
                return false;
            }
        }
        public string DistanceFromUser
        {
            get => Distance > 1500 ? "∞" : Distance.ToString();
        }

        public double CalculateDistanceFromUser()
        {
            if (Latitude == null || Longitude == null || Settings.Position is null)
            {
                return -1;
            }
            return Math.Round(Location.CalculateDistance(Settings.Position.Latitude , Settings.Position.Longitude , (double)Latitude , (double)Longitude , DistanceUnits.Kilometers) , 1);
        }


        public async Task NavigateToDetailsAsync()
        {
            if (!RequiredChecks.HasInternetConnection())
            {

                IToastMessage toastMessage = new Toaster();
                await toastMessage.MakeToastAsync(StandardToastMessages.No_Internet);
                return;
            }

            ItemDetailsPage detailsPage = new ItemDetailsPage();
            var contentService = new ContentService();

            var poi = contentService.Get(Id);

            ItemDetailsViewModel itemDetailsViewModel = new ItemDetailsViewModel(poi);
            detailsPage.BindingContext = itemDetailsViewModel;

            await Shell.Current.Navigation.PushAsync(detailsPage , true).ConfigureAwait(false);

        }

#nullable enable
        public string? Category
        {
            get => GetCategory();
            //set
            //{
            //    Category = value;
            //}
        }
#nullable disable

        private int GetTags()
        {
            ContentService contentService = new ContentService();
            var element = contentService.Get(this.Id);

            foreach (var tag in element.Tags)
            {
                if (tag.Id >= 1 && tag.Id <= 9)
                {
                    //this.Category = tag.Name;
                    return tag.Id;
                }
            }

            return 0;
        }

        private string GetCategory()
        {
            ContentService contentService = new ContentService();
            var element = contentService.Get(this.Id);

            foreach (var tag in element.Tags)
            {
                if (tag.Id >= 1 && tag.Id <= 9)
                {
                    return tag.Name;
                }
            }

            return string.Empty;
        }
    }
}

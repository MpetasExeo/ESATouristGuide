using ESATouristGuide.Helpers;
using ESATouristGuide.Interfaces;
using ESATouristGuide.ViewModels;
using ESATouristGuide.Views;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace ESATouristGuide.Models
{
    public class POI
    {
        public bool IsFavorite { get; set; }
        public string Description { get; set; }
        [JsonProperty("city")]
        public string Name { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }

        [JsonProperty("admin_name")]
        public string Region { get; set; }
        public Category Category { get; set; }
        public Temperatures Temperatures { get; set; }
        public int CategoryId { get; set; }
        public List<Uri> Images { get; set; }
        public Uri ImageUrl { get; set; }
        public double Distance => CalculateDistanceFromUser(Latitude , Longitude);
        public bool ShowDistance { get => Distance < 1500 ? true : false; }
        public string DistanceFromUser
        {
            get
            {
                if (Distance > 1500)
                {
                    return "∞";
                }
                else
                {
                    return Distance.ToString();
                }
            }
        }

        public double CalculateDistanceFromUser(double latitude , double longitude)
        {
            return Math.Round(Location.CalculateDistance(Settings.Position.Latitude , Settings.Position.Longitude , latitude , longitude , DistanceUnits.Kilometers) , 1);
        }

        public async Task NavigateToDetailsAsync()
        {
            // item null => do nothing  
            //if (item == null) return;

            //Έλεγχος για ίντερνετ
            if (!RequiredChecks.HasInternetConnection())
            {

                IToastMessage toastMessage = new Toaster();
                await toastMessage.MakeToastAsync(StandardToastMessages.No_Internet);

                return;
                //αν δεν έχει ίντερνετ ==> μην τρέξεις τον παρακάτω κώδικα
            }


            // PlaceDetailsPage init && binding 
            ItemDetailsPage detailsPage = new ItemDetailsPage();

            ItemDetailsViewModel itemDetailsViewModel = new ItemDetailsViewModel(this);

            detailsPage.BindingContext = itemDetailsViewModel;

            // όταν γίνουν τα προηγούμενα => κλείσε το popup
            //popup.Dismiss(popup);

            // Άνοιξε PlaceDetailsPage
            await Shell.Current.Navigation.PushAsync(detailsPage , true).ConfigureAwait(true);

        }
    }
}

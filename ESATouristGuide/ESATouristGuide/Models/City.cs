﻿using ESATouristGuide.Helpers;
using ESATouristGuide.Interfaces;
using ESATouristGuide.Resources;

using ESATouristGuide.ViewModels;

using ESATouristGuide.Views;

using Newtonsoft.Json;

using System;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace ESATouristGuide.Models
{
    public partial class City
    {
        [JsonProperty("city")]
        public string CityCity { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("iso2")]
        public string Iso2 { get; set; }

        [JsonProperty("admin_name")]
        public string AdminName { get; set; }

        [JsonProperty("capital")]
        public string Capital { get; set; }

        public Temperatures Temperatures { get; set; }
        public Uri ImageUrl { get; set; }
        public double DistanceFromUser
        {
            get
            {
                return CalculateDistanceFromUser(Lat , Lng);
            }
        }

        public Category Category { get; set; }

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


        public double CalculateDistanceFromUser(double latitude , double longitude)
        {
            return Math.Round(Location.CalculateDistance(Settings.Position.Latitude , Settings.Position.Longitude , latitude , longitude , DistanceUnits.Kilometers) , 1);
        }

    }
}

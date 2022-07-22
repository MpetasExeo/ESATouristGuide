namespace ESATouristGuide.Models
{
    //public partial class City
    //{
    //    [JsonProperty("city")]
    //    public string Name { get; set; }

    //    [JsonProperty("lat")]
    //    public double Latitude { get; set; }

    //    [JsonProperty("lng")]
    //    public double Longitude { get; set; }

    //    [JsonProperty("admin_name")]
    //    public string Region { get; set; }
    //    public Temperatures Temperatures { get; set; }
    //    public Uri ImageUrl { get; set; }
    //    public double Distance => CalculateDistanceFromUser(Latitude , Longitude);
    //    public bool ShowDistance { get => Distance < 1500 ? true : false; }
    //    public string DistanceFromUser
    //    {
    //        get
    //        {
    //            if (Distance > 1500)
    //            {
    //                return "∞";
    //            }
    //            else
    //            {
    //                return Distance.ToString();
    //            }
    //        }
    //    }

    //    public double CalculateDistanceFromUser(double latitude , double longitude)
    //    {
    //        return Math.Round(Location.CalculateDistance(Settings.Position.Latitude , Settings.Position.Longitude , latitude , longitude , DistanceUnits.Kilometers) , 1);
    //    }
    //    public Category Category { get; set; }

    //    public async Task NavigateToDetailsAsync()
    //    {
    //        // item null => do nothing  
    //        //if (item == null) return;

    //        //Έλεγχος για ίντερνετ
    //        if (!RequiredChecks.HasInternetConnection())
    //        {

    //            IToastMessage toastMessage = new Toaster();
    //            await toastMessage.MakeToastAsync(StandardToastMessages.No_Internet);

    //            return;
    //            //αν δεν έχει ίντερνετ ==> μην τρέξεις τον παρακάτω κώδικα
    //        }


    //        // PlaceDetailsPage init && binding 
    //        ItemDetailsPage detailsPage = new ItemDetailsPage();

    //        ItemDetailsViewModel itemDetailsViewModel = new ItemDetailsViewModel(this);

    //        detailsPage.BindingContext = itemDetailsViewModel;

    //        // όταν γίνουν τα προηγούμενα => κλείσε το popup
    //        //popup.Dismiss(popup);

    //        // Άνοιξε PlaceDetailsPage
    //        await Shell.Current.Navigation.PushAsync(detailsPage , true).ConfigureAwait(true);

    //    }

    //}
}

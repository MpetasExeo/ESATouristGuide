
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ESATouristGuide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoogleMapsPage : ContentPage
    {
        public GoogleMapsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}
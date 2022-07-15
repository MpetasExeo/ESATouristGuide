using Xamarin.Forms;

using ESATouristGuide.Views;

namespace ESATouristGuide
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(GoogleMapsPage) , typeof(GoogleMapsPage));
        }

    }
}


using ESATouristGuide.Helpers;
using ESATouristGuide.Interfaces;
using ESATouristGuide.Services;

using System.Threading;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ESATouristGuide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeTabsPage : ContentPage
    {
        public HomeTabsPage()
        {
            InitializeComponent();


            if (VersionTracking.IsFirstLaunchEver)
            {
                Navigation.PushModalAsync(new OnboardingPage());
            }

           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Settings.Position is null)
            {

                IUserLocationService _userLocationService = new UserLocationService();

                CancellationToken _ct = new CancellationToken();
                Task.Run(
                    async () =>
                    {
                        await _userLocationService.GetUserLocationAsync(_ct).ConfigureAwait(false);
                    });
            }
        }
    }
}
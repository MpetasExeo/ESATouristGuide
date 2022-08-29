using ESATouristGuide.Helpers;
using ESATouristGuide.Interfaces;
using ESATouristGuide.Models;
using ESATouristGuide.Services;
using ESATouristGuide.Views;

using System.Threading;
using System.Threading.Tasks;

using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace ESATouristGuide
{
    public partial class App : Application
    {
        public static ILocationUpdateService LocationUpdateService => DependencyService.Get<ILocationUpdateService>();

        public App()
        {
            InitializeComponent();

            Sharpnado.Tabs.Initializer.Initialize(loggerEnable: false , true);
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            DevExpress.XamarinForms.Editors.Initializer.Init();
            DevExpress.XamarinForms.Navigation.Initializer.Init();

            if (!RequiredChecks.HasInternetConnection())
            {
                MainPage = new LandingPage();
            }
            else
            {
                MainPage = new AppShell();
            }

            

            LocationUpdateService.LocationChanged += LocationUpdateService_LocationChanged;
        }

        private void LocationUpdateService_LocationChanged(object sender , ILocationEventArgs e)
        {
            //Here you can get the user's location from "e" -> new Location(e.Latitude, e.Longitude);
            //new Location is from Xamarin.Essentials Location object.
            Settings.Position = new Location(e.Latitude , e.Longitude);
        }



        protected override void OnStart()
        {
            VersionTracking.Track();
            try
            {

                OnResume();
            }
            catch (System.Exception ex)
            {
                var st = ex.Message;
                throw;
            }
        }
        protected override void OnSleep()
        {
            TheLanguage.SetLanguage();
            TheTheme.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            try
            {
                TheLanguage.SetLanguage();
                TheTheme.SetTheme();
                RequestedThemeChanged += App_RequestedThemeChanged;
            }
            catch (System.Exception ex)
            {
                var st = ex.Message;
            }

        }

        private void App_RequestedThemeChanged(object sender , AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TheTheme.SetTheme();
            });
        }
    }
}


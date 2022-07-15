using ESATouristGuide.Helpers;
using ESATouristGuide.Models;

using Xamarin.Essentials;
using Xamarin.Forms;

using ESATouristGuide.Views;

namespace ESATouristGuide
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

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
        }

        protected override void OnStart()
        {
            OnResume();
        }
        protected override void OnSleep()
        {
            TheLanguage.SetLanguage();
            TheTheme.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            TheLanguage.SetLanguage();
            TheTheme.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }

        private void App_RequestedThemeChanged( object sender , AppThemeChangedEventArgs e )
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TheTheme.SetTheme();
            });
        }
    }
}


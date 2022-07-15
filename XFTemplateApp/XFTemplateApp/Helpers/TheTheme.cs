using Xamarin.Forms;

using XFTemplateApp.Interfaces;

namespace XFTemplateApp.Helpers
{
    public static class TheTheme
    {
        public static void SetTheme()
        {
            switch (Settings.Theme)
            {
                //default
                case 0:
                    App.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
                //light
                case 1:
                    App.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                //dark
                case 2:
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
            }

            NavigationPage nav = App.Current.MainPage as Xamarin.Forms.NavigationPage;

            IEnvironment e = DependencyService.Get<IEnvironment>();
            if (App.Current.RequestedTheme == OSAppTheme.Dark)
            {
                e?.SetStatusBarColor(Color.FromHex("#0D1117") , false);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Color.FromHex("#0D1117");
                    nav.BarTextColor = Color.FromHex("#d2a8ff");
                }

            }
            else
            {

                e?.SetStatusBarColor(Color.FromHex("#ffffff") , true);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Color.FromHex("#ffffff");
                    nav.BarTextColor = Color.FromHex("#0E5DAB");
                }
            }
        }
    }
}

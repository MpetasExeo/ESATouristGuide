
using ESATouristGuide.Interfaces;

using Xamarin.Forms;

namespace ESATouristGuide.Helpers
{
    public static class TheTheme
    {
        public static void SetTheme()
        {
            switch (Settings.Theme)
            {
                //default
                case 0:
                    Application.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
                //light
                case 1:
                    Application.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                //dark
                case 2:
                    Application.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
            }

            NavigationPage nav = Application.Current.MainPage as NavigationPage;

            IEnvironment e = DependencyService.Get<IEnvironment>();
            if (Application.Current.RequestedTheme == OSAppTheme.Dark)
            {
                e?.SetStatusBarColor(Color.FromHex("#15181f") , false);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Color.FromHex("#15181f");
                    nav.BarTextColor = Color.FromHex("#d2a8ff");
                }

            }
            else
            {

                e?.SetStatusBarColor(Color.FromHex("#004080") , true);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Color.FromHex("#004080");
                    nav.BarTextColor = Color.FromHex("#ffffff");
                }
            }
        }
    }
}

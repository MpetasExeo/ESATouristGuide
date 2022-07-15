using Xamarin.Forms;

using XFTemplateApp.Helpers;
using XFTemplateApp.Interfaces;

namespace XFTemplateApp.Models
{
    public static class RequiredChecks
    {
        public static bool HasInternetConnection() => Xamarin.Essentials.Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet;
        public static bool HasGPSServiceEnabled() => DependencyService.Get<IGPSEnabled>().IsGPSEnabled();

        public static void DetailsPageRequiredChecks()
        {
            if (!HasInternetConnection())
                UserExperiencePrompts.NoInternetConnectionPrompt();

            if (!HasGPSServiceEnabled())
                UserExperiencePrompts.NoGPSPrompt();
        }
    }
}

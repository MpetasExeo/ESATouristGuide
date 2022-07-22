using Android.Content;

using ESATouristGuide.Droid.Helpers;
using ESATouristGuide.Interfaces;

using Xamarin.Forms;

[assembly: Dependency(typeof(GpsEnabledService))]
namespace ESATouristGuide.Droid.Helpers
{
    public class GpsEnabledService : IGPSEnabled
    {
        public bool IsGPSEnabled()
        {
            var value = false;
            Android.Locations.LocationManager manager = (Android.Locations.LocationManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.LocationService);
            if (!manager.IsProviderEnabled(Android.Locations.LocationManager.GpsProvider))
            {
                //gps disable
                return value;
            }

            value = true;
            return value;

        }

        public void OpenSettings()
        {
            Intent intent = new Android.Content.Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
        }

    }
}
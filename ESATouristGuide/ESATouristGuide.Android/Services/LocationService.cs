using Android.Content;
using Android.Locations;

using ESATouristGuide.Interfaces;

using System;

namespace ESATouristGuide.Droid.Services
{
    internal class LocationService : IPlatformSpecificLocationService
    {
        public bool IsLocationServiceEnabled()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);

            try
            {
                return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool OpenDeviceLocationSettingsPage()
        {
            Intent intent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
            return true;
        }
    }
}
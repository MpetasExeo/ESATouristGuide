using ESATouristGuide.Interfaces;

using System.Threading.Tasks;

using Xamarin.Essentials;

namespace ESATouristGuide.Services
{
    internal class LocationService : ILocationService
    {
        public async Task<bool> IsLocationPermissionGranted()
        {
            if (await Permissions.CheckStatusAsync<Permissions.LocationAlways>() == PermissionStatus.Granted ||
                await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>() == PermissionStatus.Granted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<PermissionStatus> RequestLocationPermission()
        {
            return await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        public async Task<PermissionStatus> CheckLocationPermission()
        {
            return await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        }

        public async Task<Location> GetCurrentLocation()
        {
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.High);

            var location = await Geolocation.GetLocationAsync(request);

            if (location == null)
            {
                return null;
            }

            if (location.IsFromMockProvider)
            {
                // location is from a fake provider. May not be correct.
                // See more - https://www.xda-developers.com/fake-android-location/
                Location result = null;
#if DEBUG
                result = location;
#endif
                return result;
            }
            else
            {
                return location;
            }
        }
    }
}

using System.Threading.Tasks;

using Xamarin.Essentials;

namespace ESATouristGuide.Interfaces
{
    internal interface ILocationService
    {
        Task<Location> GetCurrentLocation();
        Task<PermissionStatus> RequestLocationPermission();
        Task<PermissionStatus> CheckLocationPermission();
        Task<bool> IsLocationPermissionGranted();
    }
}

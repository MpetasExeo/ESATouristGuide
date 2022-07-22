using CoreLocation;

using ESATouristGuide.Interfaces;

using System;

using XFTemplateApp.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(LocationUpdateService))]
namespace XFTemplateApp.iOS.Services
{

    public class LocationEventArgs : EventArgs, ILocationEventArgs
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class LocationUpdateService : ILocationUpdateService
    {
        private CLLocationManager _locationManager;

        public event EventHandler<ILocationEventArgs> LocationChanged;

        event EventHandler<ILocationEventArgs> ILocationUpdateService.LocationChanged
        {
            add
            {
                LocationChanged += value;
            }
            remove
            {
                LocationChanged -= value;
            }
        }

        public void GetUserLocation()
        {
            _locationManager = new CLLocationManager
            {
                DesiredAccuracy = CLLocation.AccuracyBest ,
                DistanceFilter = CLLocationDistance.FilterNone
            };

            _locationManager.LocationsUpdated +=
                (object sender , CLLocationsUpdatedEventArgs e) =>
                {
                    var locations = e.Locations;
                    var strLocation = locations[locations.Length - 1].Coordinate.Latitude.ToString();

                    strLocation = strLocation + "," + locations[locations.Length - 1].Coordinate.Longitude.ToString();

                    LocationEventArgs args = new LocationEventArgs();
                    args.Latitude = locations[locations.Length - 1].Coordinate.Latitude;
                    args.Longitude = locations[locations.Length - 1].Coordinate.Longitude;

                    LocationChanged(this , args);
                };

            _locationManager.AuthorizationChanged += (object sender , CLAuthorizationChangedEventArgs e) =>
            {
                if (e.Status ==
                    CLAuthorizationStatus.AuthorizedWhenInUse)
                {
                    _locationManager.StartUpdatingLocation();
                }
            };

            _locationManager.RequestWhenInUseAuthorization();
        }

        ~LocationUpdateService()
        {
            _locationManager.StopUpdatingLocation();
        }
    }
}
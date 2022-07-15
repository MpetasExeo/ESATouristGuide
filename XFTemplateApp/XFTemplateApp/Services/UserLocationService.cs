using System;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

using XFTemplateApp.Helpers;
using XFTemplateApp.Interfaces;

namespace XFTemplateApp.Services
{
    public class UserLocationService : IUserLocationService
    {
        CancellationTokenSource cts;
        readonly Position DummyPosition = new Position(40.5000001 , 22.9500001);

        public async Task<Position> GetUserLocationAsync( CancellationToken cancellationToken )
        {
            PermissionStatus locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().ConfigureAwait(true);

            if (locationPermissionStatus == PermissionStatus.Granted)
            {
                try
                {
                    //Location location = await Geolocation.GetLastKnownLocationAsync();
                    Position pos = new Position();

                    //if (location != null)
                    //{
                    //    pos = new Position(location.Latitude , location.Longitude);

                    //    if (pos != DummyPosition)
                    //    {
                    //        Settings.Position = pos;
                    //    }

                    //    return location != null ? pos : DummyPosition;
                    //}
                    //else
                    {
                        GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium , TimeSpan.FromSeconds(8));
                        cts = new CancellationTokenSource();
                        var location = await Geolocation.GetLocationAsync(request , cts.Token).ConfigureAwait(true);

                        pos = new Position(location.Latitude , location.Longitude);

                        if (pos != DummyPosition)
                        {
                            Settings.Position = pos;
                        }

                        return location != null ? pos : DummyPosition;

                    }
                }
#pragma warning disable CS0168 // The variable 'fnsEx' is declared but never used
                catch (FeatureNotSupportedException fnsEx)
#pragma warning restore CS0168 // The variable 'fnsEx' is declared but never used
                {
                    //await NoGPSEnabled();
                    return DummyPosition;

                }
#pragma warning disable CS0168 // The variable 'fneEx' is declared but never used
                catch (FeatureNotEnabledException fneEx)
#pragma warning restore CS0168 // The variable 'fneEx' is declared but never used
                {
                    //await NoGPSEnabled();
                    return DummyPosition;

                }
#pragma warning disable CS0168 // The variable 'pEx' is declared but never used
                catch (PermissionException pEx)
#pragma warning restore CS0168 // The variable 'pEx' is declared but never used
                {
                    //await NoGPSEnabled();
                    return DummyPosition;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    return DummyPosition;
                }
            }
            else
            {
                try
                {
                    Device.BeginInvokeOnMainThread(async () => await Permissions.RequestAsync<Permissions.LocationWhenInUse>().ConfigureAwait(true));
                    return DummyPosition;
                }
                catch (Exception)
                {
                    return DummyPosition;
                }
            }
        }
    }


}

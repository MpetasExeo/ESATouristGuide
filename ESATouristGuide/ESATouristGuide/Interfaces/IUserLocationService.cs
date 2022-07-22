﻿
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace ESATouristGuide.Interfaces
{
    public interface IUserLocationService
    {
        Task<Location> GetUserLocationAsync( CancellationToken cancellationToken );
    }

}

using ESATouristGuide.Models;

using System.Threading;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace ESATouristGuide.Interfaces
{
    public interface IWeatherService
    {
        Task<Temperatures> GetCurrentWeatherAsync( Location location , CancellationToken cancellationToken );
    }
}

using ESATouristGuide.Models;

using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms.GoogleMaps;

namespace ESATouristGuide.Interfaces
{
    public interface IWeatherService
    {
        Task<Temperatures> GetCurrentWeatherAsync( Position position , CancellationToken cancellationToken );
    }
}

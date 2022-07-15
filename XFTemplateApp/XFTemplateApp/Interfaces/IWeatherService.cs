using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms.GoogleMaps;

using XFTemplateApp.Models;

namespace XFTemplateApp.Interfaces
{
    public interface IWeatherService
    {
        Task<Temperatures> GetCurrentWeatherAsync( Position position , CancellationToken cancellationToken );
    }
}

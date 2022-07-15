using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms.GoogleMaps;

namespace XFTemplateApp.Interfaces
{
    public interface IUserLocationService
    {
        Task<Position> GetUserLocationAsync( CancellationToken cancellationToken );
    }

}

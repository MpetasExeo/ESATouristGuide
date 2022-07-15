using System.Threading.Tasks;

using Xamarin.Forms.GoogleMaps;

using XFTemplateApp.Models;

namespace XFTemplateApp.Interfaces
{
    public interface IDistancesService
    {
        Task<Distances> GetDistancesFromUserAsync( Position PlacePosition , Position UserPosition );
    }
}

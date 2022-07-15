
using ESATouristGuide.Models;

using System.Threading.Tasks;

using Xamarin.Forms.GoogleMaps;

namespace ESATouristGuide.Interfaces
{
    public interface IDistancesService
    {
        Task<Distances> GetDistancesFromUserAsync( Position PlacePosition , Position UserPosition );
    }
}

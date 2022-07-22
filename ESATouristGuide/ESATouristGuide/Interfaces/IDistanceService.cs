
using ESATouristGuide.Models;

using System.Threading.Tasks;

using Xamarin.Essentials;

namespace ESATouristGuide.Interfaces
{
    public interface IDistancesService
    {
        Task<Distances> GetDistancesFromUserAsync(Location PlacePosition , Location UserPosition);
    }
}

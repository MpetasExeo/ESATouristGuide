using ESATouristGuide.Models;

using MvvmHelpers;

using System.Threading.Tasks;

namespace ESATouristGuide.Interfaces
{
    public interface IGreekCitiesService
    {
        /// <summary>
        /// Returns the cities deserialized from the json file.
        /// </summary>
        /// <returns></returns>
        Task<ObservableRangeCollection<City>> GetGreekCities();
    }
}

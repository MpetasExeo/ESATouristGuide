using MvvmHelpers;

using System.Threading.Tasks;

using XFTemplateApp.Models;

namespace XFTemplateApp.Interfaces
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

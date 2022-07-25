using ESATouristGuide.Models;

using MvvmHelpers;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESATouristGuide.Interfaces
{
    public interface IGreekCitiesService
    {
        /// <summary>
        /// Returns the cities deserialized from the json file.
        /// </summary>
        /// <returns></returns>
        Task<ObservableRangeCollection<POI>> GetGreekCities();


        Task<List<POI>> GetPagedListItem(
            int tabId ,
            int[] category ,
            int? page ,
            int pageSize = 10 ,
            int window = 5 ,
            bool ascending = true);
        
        POI Get(int id);


    }
}

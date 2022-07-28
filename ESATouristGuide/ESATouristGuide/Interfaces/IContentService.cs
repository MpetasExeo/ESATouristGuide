using ESATouristGuide.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace ESATouristGuide.Interfaces
{
    public interface IContentService
    {
        PagedList<POISlim> GetPagedListItem(int tabId , int[] category , int? page , int pageSize = 10 , int window = 5 , bool ascending = true);
        POI Get(int id);
        //List<ARItem> GetArItems(Position userLocation , double distance);
    }
}

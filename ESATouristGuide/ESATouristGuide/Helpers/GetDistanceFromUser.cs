
using System;

using Xamarin.Essentials;

namespace ESATouristGuide.Helpers
{
    public class GetDistanceFromUser
    {
        public string CalculateDistanceFromUser( double latitude , double longitude )
        {
            return Math.Round(Location.CalculateDistance(Settings.Position.Latitude , Settings.Position.Longitude , latitude , longitude , DistanceUnits.Kilometers) , 1).ToString();
        }
    }
}

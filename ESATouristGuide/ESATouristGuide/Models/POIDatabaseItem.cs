using SQLite;

using System;

namespace ESATouristGuide.Models
{
    public class POIDatabaseItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public Uri ImageUrl { get; set; }
    }
}

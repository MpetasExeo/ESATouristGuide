using ESATouristGuide.Helpers;
using ESATouristGuide.Interfaces;
using ESATouristGuide.ViewModels;
using ESATouristGuide.Views;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace ESATouristGuide.Models
{
    public class POI : POISlim
    {
        public bool IsFavorite { get; set; }
        //public Category Category { get; set; }
        public Temperatures Temperatures { get; set; }
        public List<CategorySlim> Tags { get; set; }
        public List<Uri> Images { get; set; }
    }
}

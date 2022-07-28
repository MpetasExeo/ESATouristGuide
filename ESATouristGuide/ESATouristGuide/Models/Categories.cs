
using ESATouristGuide.Resources;

using System.Collections.Generic;

namespace ESATouristGuide.Models
{
    public static class Categories
    {
        private static readonly List<Category> _categoriesList = new List<Category>
        {
            new Category {Id=5 ,Color = "#2f962f", Text = "Φύση", Image = "place.png" },
            new Category {Id=6 ,Color = "#748f53", Text = "Δραστηριότητες", Image = "seaandsun.png" },
            new Category {Id=7 ,Color = "#914c2f", Text = AppResources.Gastronomy, Image = "gastronomy.png" },
            new Category {Id=8 ,Color = "#a38b3b", Text = "Εκδηλώσεις", Image = "place.png" },
            new Category {Id=4 ,Color = "#2b5f82", Text = "Παραλίες", Image = "outdoor.png" },
            new Category {Id=3 ,Color = "#abcf2e", Text = "Μουσεία", Image = "culture.png" },
            new Category {Id=2 ,Color = "#b83737", Text = "Μνημεία", Image = "culture.png" },
            new Category {Id=1 ,Color = "#9c436f", Text = "Κορυφαία αξιοθέατα", Image = "culture.png" },
            new Category {Id=9 ,Color = "#4cf3f6", Text = "Χρήσιμα", Image = "culture.png" }
        };

        public static List<Category> CategoriesList => _categoriesList;
    }
}

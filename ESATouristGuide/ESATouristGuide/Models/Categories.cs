
using ESATouristGuide.Resources;

using System.Collections.Generic;

namespace ESATouristGuide.Models
{
    public static class Categories
    {
        private static readonly List<Category> _categoriesList = new List<Category>
        {
            new Category { Color = "#2f962f", Text = "Φύση", Image = "place.png" },
            new Category { Color = "#748f53", Text = "Δραστηριότητες", Image = "seaandsun.png" },
            new Category { Color = "#914c2f", Text = AppResources.Gastronomy, Image = "gastronomy.png" },
            new Category { Color = "#a38b3b", Text = "Εκδηλώσεις", Image = "place.png" },
            new Category { Color = "#2b5f82", Text = "Παραλίες", Image = "outdoor.png" },
            new Category { Color = "#abcf2e", Text = "Μουσεία", Image = "culture.png" },
            new Category { Color = "#b83737", Text = "Μνημεία", Image = "culture.png" }
            ,new Category { Color = "#9c436f", Text = "Κορυφαία αξιοθέατα", Image = "culture.png" }


        };

        public static List<Category> CategoriesList => _categoriesList;
    }
}

using System.Collections.Generic;

using XFTemplateApp.Resources;

namespace XFTemplateApp.Models
{
    public static class Categories
    {
        private static readonly List<Category> categoriesList = new List<Category>
        {
            new Category { Color = "#abcf2e", Text = AppResources.PlacesToGo, Image = "place.png" },
            new Category { Color = "#abcf2e", Text = AppResources.SeaAndSun, Image = "seaandsun.png" },
            new Category { Color = "#abcf2e", Text = AppResources.Gastronomy, Image = "gastronomy.png" },
            new Category { Color = "#abcf2e", Text = AppResources.ArtLife, Image = "place.png" },
            new Category { Color = "#abcf2e", Text = AppResources.Outdoor, Image = "outdoor.png" },
            new Category { Color = "#abcf2e", Text = AppResources.Politismos, Image = "culture.png" }
        };

        public static List<Category> CategoriesList => categoriesList;
    }
}

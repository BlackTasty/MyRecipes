using MyRecipes.Core;
using MyRecipes.Core.Enum;
using MyRecipes.Core.Recipes;
using MyRecipes.Core.SeasonCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    static class TestData
    {
        public static Recipe GetTestRecipe(string name)
        {
            return new Recipe("", name, "This is a test", DateTime.Now,
                new List<RecipeIngredient>()
                {
                    new RecipeIngredient(GetTestIngredient("Mehl", "https://www.youtube.com/feed/subscriptions"), 500, Unit.Gram),
                    new RecipeIngredient(GetTestIngredient("Wasser"), 300, Unit.Milliliters),
                    new RecipeIngredient(GetTestIngredient("Backpulver"), 2, Unit.Package)
                },
                new List<string>()
                {
                    "Mehl und Wasser mischen",
                    "Backpulver essen",
                    "Genießen"
                },
                GetTestCategories(),
                new RecipeImage(@"D:\Program Files (x86)\Steam\userdata\96277141\760\remote\588650\screenshots\20210406161102_1.jpg"), 
                6);
        }

        public static Ingredient GetTestIngredient(string name, string link = "")
        {
            return new Ingredient("", name, "", link, DateTime.Now, new List<Season>(), IngredientCategory.Unset, 
                Unit.Gram, null);
        }

        public static Ingredient GetTestIngredient(string name, List<Season> seasons, string link = "")
        {
            return new Ingredient("", name, "", link, DateTime.Now, seasons, IngredientCategory.Unset,
                Unit.Gram, null);
        }

        public static List<Category> GetTestCategories()
        {
            return new List<Category>()
            {
                new Category("Backen"),
                new Category("Österr. Küche")
            };
        }

        public static List<FilterObject> GetTestFilterCategories()
        {
            return new List<FilterObject>()
            {
                new FilterObject("Backen"),
                new FilterObject("Österr. Küche")
            };
        }
    }
}

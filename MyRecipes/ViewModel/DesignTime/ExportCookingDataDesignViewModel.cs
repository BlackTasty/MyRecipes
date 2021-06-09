using MyRecipes.Core.Export;
using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    class ExportCookingDataDesignViewModel : ExportCookingDataViewModel
    {
        public ExportCookingDataDesignViewModel()
        {
            Ingredients = new ExportSet<Ingredient>(new List<Ingredient>()
            {
                TestData.GetTestIngredient("Mehl"),
                TestData.GetTestIngredient("Eier"),
                TestData.GetTestIngredient("Milch")
            }, true);

            Categories = new ExportSet<Category>(TestData.GetTestCategories(), true);
        }
    }
}

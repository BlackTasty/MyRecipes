using MyRecipes.Core.Export;
using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    class ImportRecipeDesignViewModel : ImportRecipeViewModel
    {
        public ImportRecipeDesignViewModel()
        {
            Recipe selected = new Recipe("Pancakes");
            selected.Categories.AddRange(TestData.GetTestCategories());

            Recipes = new ExportSet<Recipe>(new List<Recipe>()
            {
                new Recipe("Brathuhn"),
                selected,
                new Recipe("Curry"),
                new Recipe("Schnitzel")
            }, true);
        }
    }
}

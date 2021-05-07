using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    class RecipeViewDesignViewModel : RecipeViewViewModel
    {
        public RecipeViewDesignViewModel()
        {
            Recipe = TestData.GetTestRecipe("Test");
        }
    }
}

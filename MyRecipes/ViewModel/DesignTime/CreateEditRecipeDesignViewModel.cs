using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    class CreateEditRecipeDesignViewModel : CreateEditRecipeViewModel
    {
        public CreateEditRecipeDesignViewModel()
        {
            Recipe = TestData.GetTestRecipe("Test");
            AvailableIngredients.Add(new Ingredient("Mehl"));
            AvailableIngredients.Add(new Ingredient("Milch"));
            AvailableIngredients.Add(new Ingredient("Butter"));
            AvailableIngredients.Add(new Ingredient("Eier"));
            AvailableIngredients.Add(new Ingredient("Salz"));
            AvailableIngredients.Add(new Ingredient("Zucker"));

            Recipe.Categories.AddRange(TestData.GetTestCategories());

            SelectedIngredientIndex = 2;
            IngredientAmount = 80;
            SelectedMeasurementTypeIndex = 2;
        }
    }
}

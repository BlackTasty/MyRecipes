using MyRecipes.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    class RecipeListDesignViewModel : RecipeListViewModel
    {
        public RecipeListDesignViewModel()
        {
            AvailableRecipes.Add(TestData.GetTestRecipe("Eierspeis"));
            AvailableRecipes.Add(TestData.GetTestRecipe("Schnitzel"));
            AvailableRecipes.Add(TestData.GetTestRecipe("Curry"));
            AvailableRecipes.Add(TestData.GetTestRecipe("Brathendl"));
            AvailableRecipes.Add(TestData.GetTestRecipe("Pancakes"));

            AvailableIngredients.Add(new FilterObject(TestData.GetTestIngredient("Mehl")));
            AvailableIngredients.Add(new FilterObject(TestData.GetTestIngredient("Milch")));
            AvailableIngredients.Add(new FilterObject(TestData.GetTestIngredient("Eier")));

            SelectedIngredients.Add(AvailableIngredients[1]);
            SelectedIngredients.Add(AvailableIngredients[2]);

            AvailableCategories.AddRange(TestData.GetTestFilterCategories());

            SelectedCategory = AvailableCategories[0];

            SearchByRecipe = false;
            SearchByIngredient = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    class MainDesignViewModel : MainViewModel
    {
        public MainDesignViewModel()
        {
            ShowBackToIngredientsButton = true;
            ShowBackToRecipesButton = true;
            ShowSaisonCalendarButton = true;
            ShoppingList = new Core.Recipes.ShoppingList();
            ShoppingList.AddRecipe(TestData.GetTestRecipe("Test"));
            ShowShoppingList = true;
        }
    }
}

using MyRecipes.Core.Enum;
using MyRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;
using Tasty.ViewModel.JsonNet;

namespace MyRecipes.Core.Recipes
{
    class ShoppingList : ViewModelBase
    {
        private JsonObservableCollection<ShoppingRecipe> mSelectedRecipes = new JsonObservableCollection<ShoppingRecipe>("SelectedRecipes", true);

        public JsonObservableCollection<ShoppingRecipe> SelectedRecipes
        {
            get => mSelectedRecipes;
            set
            {
                mSelectedRecipes = value;
                InvokePropertyChanged();
                InvokePropertyChanged("IsEmpty");
            }
        }

        public bool IsEmpty => mSelectedRecipes.Count == 0;

        public void AddRecipe(Recipe recipe)
        {
            mSelectedRecipes.Add(new ShoppingRecipe(recipe));
            InvokePropertyChanged("IsEmpty");
        }

        /*public void AddRecipe(Recipe recipe)
        {
            List<RecipeIngredient> ingredients = new List<RecipeIngredient>();
            ingredients.Add(recipe);
            foreach (RecipeIngredient ingredient in recipe.Ingredients)
            {
                int index = ingredients.FindIndex(x => x.Ingredient.Name == ingredient.Ingredient.Name && 
                                            x.MeasurementType == ingredient.MeasurementType);
                if (index > -1)
                {
                    ingredients[index].Amount += ingredient.Amount;
                }
                else
                {
                    ingredients.Add(ingredient);
                }
            }

            Clear();
            mSelectedRecipes.AddRange(ingredients.OrderBy(x => x.Ingredient.Name));
            InvokePropertyChanged("IsEmpty");
        }*/

        public void Clear()
        {
            if (mSelectedRecipes.Count > 0)
            {
                mSelectedRecipes.Clear();
                InvokePropertyChanged("IsEmpty");
            }
        }
    }
}

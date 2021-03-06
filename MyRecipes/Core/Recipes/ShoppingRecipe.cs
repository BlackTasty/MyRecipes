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
    class ShoppingRecipe : ViewModelBase
    {
        private Recipe mRecipe;
        private int mServings;
        private JsonObservableCollection<RecipeIngredient> mIngredients = new JsonObservableCollection<RecipeIngredient>("Ingredients", true);

        public Recipe Recipe
        {
            get => mRecipe;
            set
            {
                mRecipe = value;
                if (value != null)
                {
                    Servings = value.Servings;
                }
                else
                {
                    Servings = 1;
                }
                InvokePropertyChanged();
            }
        }

        public int Servings
        {
            get => mServings;
            set
            {
                mServings = Math.Max(1, value);
                InvokePropertyChanged();

                Ingredients.Clear();
                if (mRecipe != null)
                {
                    List<RecipeIngredient> ingredients = new List<RecipeIngredient>();

                    foreach (RecipeIngredient ingredient in mRecipe.Ingredients)
                    {
                        ingredients.Add(ingredient.FromServingRatio(mRecipe.Servings, value));
                    }
                    Ingredients.AddRange(ingredients);
                }
            }
        }

        public JsonObservableCollection<RecipeIngredient> Ingredients
        {
            get => mIngredients;
            set
            {
                mIngredients = value;
                InvokePropertyChanged();
            }
        }

        public ShoppingRecipe(Recipe recipe)
        {
            Recipe = recipe;
        }
    }
}

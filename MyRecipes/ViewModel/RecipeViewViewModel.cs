using MyRecipes.Core.Recipes;
using MyRecipes.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel
{
    class RecipeViewViewModel : ViewModelBase
    {
        private OpenUrlCommand mOpenUrlCommand = new OpenUrlCommand();
        private Recipe mRecipe;
        private int mServings;
        private VeryObservableCollection<RecipeIngredient> mIngredients = new VeryObservableCollection<RecipeIngredient>("Ingredients");

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

        public VeryObservableCollection<RecipeIngredient> Ingredients
        {
            get => mIngredients;
            set
            {
                mIngredients = value;
                InvokePropertyChanged();
            }
        }

        public OpenUrlCommand OpenUrlCommand => mOpenUrlCommand;
    }
}

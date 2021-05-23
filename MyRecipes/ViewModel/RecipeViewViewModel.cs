using MyRecipes.Core.Observer;
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
        public event EventHandler<ChangeObservedEventArgs> RecipeChanged;

        protected OpenUrlCommand mOpenUrlCommand = new OpenUrlCommand();
        protected Recipe mRecipe;
        protected int mServings;
        protected VeryObservableCollection<RecipeIngredient> mIngredients = new VeryObservableCollection<RecipeIngredient>("Ingredients");

        public Recipe Recipe
        {
            get => mRecipe;
            set
            {
                if (mRecipe != null)
                {
                    mRecipe.ChangeObserved -= Recipe_ChangeObserved;
                }

                mRecipe = value;
                if (value != null)
                {
                    Servings = value.Servings;
                    mRecipe.ChangeObserved += Recipe_ChangeObserved;
                }
                else
                {
                    Servings = 1;
                }
                InvokePropertyChanged();
            }
        }

        private void Recipe_ChangeObserved(object sender, ChangeObservedEventArgs e)
        {
            OnRecipeChanged(new ChangeObservedEventArgs(Recipe.UnsavedChanges, e.NewValue, e.Observer));
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

        protected virtual void OnRecipeChanged(ChangeObservedEventArgs e)
        {
            RecipeChanged?.Invoke(this, e);
        }
    }
}

using MyRecipes.Core.Enum;
using MyRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Recipes
{
    class ShoppingList : ViewModelBase
    {
        private VeryObservableCollection<RecipeIngredient> mRequiredIngredients = new VeryObservableCollection<RecipeIngredient>("RequiredIngredients");

        public VeryObservableCollection<RecipeIngredient> RequiredIngredients
        {
            get => mRequiredIngredients;
            set
            {
                mRequiredIngredients = value;
                InvokePropertyChanged();
                InvokePropertyChanged("IsEmpty");
            }
        }

        public bool IsEmpty => mRequiredIngredients.Count == 0;

        public void AddRecipe(Recipe recipe)
        {
            List<RecipeIngredient> ingredients = new List<RecipeIngredient>();
            ingredients.AddRange(mRequiredIngredients);
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
            mRequiredIngredients.AddRange(ingredients.OrderBy(x => x.Ingredient.Name));
            InvokePropertyChanged("IsEmpty");
        }

        public void Clear()
        {
            if (mRequiredIngredients.Count > 0)
            {
                mRequiredIngredients.Clear();
                InvokePropertyChanged("IsEmpty");
            }
        }
    }
}

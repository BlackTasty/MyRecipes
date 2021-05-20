using MyRecipes.Core.Enum;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Recipes
{
    public class RecipeIngredient : ViewModelBase
    {
        private Ingredient mIngredient;
        private double mAmount;
        private Unit mMeasurementType;

        public Ingredient Ingredient
        {
            get => mIngredient;
            set
            {
                mIngredient = value;
                InvokePropertyChanged();
            }
        }

        public double Amount
        {
            get => mAmount;
            set
            {
                mAmount = value;
                InvokePropertyChanged();
            }
        }

        public Unit MeasurementType
        {
            get => mMeasurementType;
            set
            {
                mMeasurementType = value;
                InvokePropertyChanged();
            }
        }

        [JsonConstructor]
        public RecipeIngredient(Ingredient ingredient, double amount, Unit measurementType)
        {
            Ingredient = ingredient;
            Amount = amount;
            MeasurementType = measurementType;
        }

        public RecipeIngredient FromServingRatio(double baseServings, double desiredServings)
        {
            double ratio = desiredServings / baseServings;

            return new RecipeIngredient(Ingredient, Math.Round(Amount * ratio, 2), MeasurementType);
        }
    }
}

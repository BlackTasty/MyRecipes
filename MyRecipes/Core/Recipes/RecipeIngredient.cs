using MyRecipes.Core.Enum;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;
using Tasty.ViewModel.Observer;

namespace MyRecipes.Core.Recipes
{
    public class RecipeIngredient : ViewModelBase
    {
        private Ingredient mIngredient;
        private double mAmount;
        private Unit mMeasurementType;
        protected ObserverManager observerManager = new ObserverManager();

        [JsonIgnore]
        public ObserverManager ObserverManager => observerManager;

        public Ingredient Ingredient
        {
            get => mIngredient;
            set
            {
                if (mIngredient != null)
                {
                    observerManager.UnregisterChild(mIngredient.ObserverManager);
                }
                mIngredient = value;
                if (value != null)
                {
                    observerManager.UnregisterChild(value.ObserverManager);
                }
                InvokePropertyChanged();
            }
        }

        public double Amount
        {
            get => mAmount;
            set
            {
                observerManager.ObserveProperty(value);
                mAmount = value;
                InvokePropertyChanged();
            }
        }

        public Unit MeasurementType
        {
            get => mMeasurementType;
            set
            {
                observerManager.ObserveProperty(value);
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

using MyRecipes.Core.Enum;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;

namespace MyRecipes.Core.Recipes
{
    public class MeasurementConversion : ViewModelBase
    {
        private Unit mSourceMeasurement;
        private Unit mTargetMeasurement;
        private double mSourceAmount;
        private double mTargetAmount;

        public Unit SourceMeasurement
        {
            get => mSourceMeasurement;
            set
            {
                mSourceMeasurement = value;
                InvokePropertyChanged();
            }
        }

        public Unit TargetMeasurement
        {
            get => mTargetMeasurement;
            set
            {
                mTargetMeasurement = value;
                InvokePropertyChanged();
            }
        }

        public double SourceAmount
        {
            get => mSourceAmount;
            set
            {
                mSourceAmount = value;
                InvokePropertyChanged();
            }
        }

        public double TargetAmount
        {
            get => mTargetAmount;
            set
            {
                mTargetAmount = value;
                InvokePropertyChanged();
            }
        }

        [JsonConstructor]
        public MeasurementConversion(Unit sourceMeasurement, double sourceAmount, 
            Unit targetMeasurement, double targetAmount)
        {
            mSourceAmount = sourceAmount;
            mSourceMeasurement = sourceMeasurement;
            mTargetAmount = targetAmount;
            mTargetMeasurement = targetMeasurement;
        }

        public MeasurementConversion(Ingredient ingredient)
        {
            mSourceAmount = 1;
            mSourceMeasurement = ingredient.MeasurementType;
            mTargetAmount = 1;
        }
    }
}

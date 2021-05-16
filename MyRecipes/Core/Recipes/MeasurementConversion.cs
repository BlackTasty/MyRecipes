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
    public class MeasurementConversion : ViewModelBase
    {
        private MeasurementType mSourceMeasurement;
        private MeasurementType mTargetMeasurement;
        private double mSourceAmount;
        private double mTargetAmount;

        public MeasurementType SourceMeasurement
        {
            get => mSourceMeasurement;
            set
            {
                mSourceMeasurement = value;
                InvokePropertyChanged();
            }
        }

        public MeasurementType TargetMeasurement
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
        public MeasurementConversion(MeasurementType sourceMeasurement, double sourceAmount, 
            MeasurementType targetMeasurement, double targetAmount)
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

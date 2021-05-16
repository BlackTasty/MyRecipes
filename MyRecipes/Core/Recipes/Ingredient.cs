﻿using MyRecipes.Core.Enum;
using MyRecipes.Core.SeasonCalendar;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Recipes
{
    public class Ingredient : BaseData<Ingredient>
    {
        private string mProductLink;
        private IngredientCategory mIngredientCategory;
        private MeasurementType mMeasurementType;
        private MeasurementConversion mMeasurementConversion;
        private VeryObservableCollection<Season> mSeasons = new VeryObservableCollection<Season>("Seasons");

        public string ProductLink
        {
            get => mProductLink;
            set
            {
                changeManager.ObserveProperty(value);
                mProductLink = value;
                InvokePropertyChanged();
            }
        }

        [JsonIgnore]
        public MeasurementConversion MeasurementConversion
        {
            get => mMeasurementConversion;
            set
            {
                mMeasurementConversion = value;
                InvokePropertyChanged();
            }
        }

        public MeasurementType MeasurementType
        {
            get => mMeasurementType;
            set
            {
                mMeasurementType = value;
                InvokePropertyChanged();
            }
        }

        public IngredientCategory IngredientCategory
        {
            get => mIngredientCategory;
            set
            {
                changeManager.ObserveProperty(value);
                mIngredientCategory = value;
                InvokePropertyChanged();
            }
        }

        public VeryObservableCollection<Season> Seasons
        {
            get => mSeasons;
            set
            {
                changeManager.ObserveProperty(value);
                mSeasons = value;
                InvokePropertyChanged();
            }
        }

        [JsonIgnore]
        public Dictionary<SeasonMonth, WareOriginType> SeasonPlan
        {
            get
            {
                Dictionary<SeasonMonth, WareOriginType> seasonPlan = new Dictionary<SeasonMonth, WareOriginType>();

                for (int i = 0; i <= 11; i++)
                {
                    if (Seasons.Any(x => x.ActiveSeasons.TryGetValue((SeasonMonth)i, out WareOriginType originType) && originType == WareOriginType.Fresh))
                    {
                        seasonPlan.Add((SeasonMonth)i, WareOriginType.Fresh);
                    }
                    else if (Seasons.Any(x => x.ActiveSeasons.TryGetValue((SeasonMonth)i, out WareOriginType originType) && originType == WareOriginType.Warehouse))
                    {
                        seasonPlan.Add((SeasonMonth)i, WareOriginType.Warehouse);
                    }
                }

                return seasonPlan;
            }
        }

        [JsonConstructor]
        public Ingredient(string guid, string name, string description, string productLink, DateTime lastModifyDate,
            List<Season> seasons, IngredientCategory ingredientCategory, MeasurementType measurementType, 
            MeasurementConversion measurementConversion) : 
            base(guid, name, description, lastModifyDate)
        {
            mProductLink = productLink;
            mIngredientCategory = ingredientCategory;
            mMeasurementType = measurementType;
            if (seasons != null)
            {
                mSeasons.AddRange(seasons);
            }
            mMeasurementConversion = measurementConversion != null ? measurementConversion : new MeasurementConversion(this);
        }

        public Ingredient(string name) : base(name)
        {

        }
    }
}

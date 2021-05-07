using MyRecipes.Core.Recipes;
using MyRecipes.Core.SeasonCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel
{
    class SeasonIngredientConfiguratorViewModel : ViewModelBase
    {
        private Ingredient mIngredient;
        private SeasonMonth mRangeStart = SeasonMonth.January;
        private SeasonMonth mRangeEnd = SeasonMonth.March;
        private WareOriginType mWareOriginType = WareOriginType.Unset;
        private bool mWholeYear;

        public Ingredient Ingredient
        {
            get => mIngredient; set
            {
                mIngredient = value;
                InvokePropertyChanged();
            }
        }

        public bool WholeYear
        {
            get => mWholeYear;
            set
            {
                mWholeYear = value;
                InvokePropertyChanged();
            }
        }

        public SeasonMonth RangeStart
        {
            get => mRangeStart;
            set
            {
                mRangeStart = value;
                InvokePropertyChanged();
            }
        }

        public SeasonMonth RangeEnd
        {
            get => mRangeEnd;
            set
            {
                mRangeEnd = value;
                InvokePropertyChanged();
            }
        }

        public WareOriginType WareOriginType
        {
            get => mWareOriginType;
            set
            {
                mWareOriginType = value;
                InvokePropertyChanged();
            }
        }
    }
}

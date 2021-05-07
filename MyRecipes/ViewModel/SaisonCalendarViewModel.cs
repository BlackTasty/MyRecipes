using MyRecipes.Core.Recipes;
using MyRecipes.Core.SeasonCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel
{
    class SaisonCalendarViewModel : ViewModelBase
    {
        private bool mSortAscending = true;

        private IOrderedEnumerable<Ingredient> seasonFruits;
        private IOrderedEnumerable<Ingredient> seasonVegetables;
        private IOrderedEnumerable<Ingredient> seasonNuts;

        public bool SortAscending
        {
            get => mSortAscending;
            set
            {
                mSortAscending = value;
                InvokePropertyChanged();
                InvokePropertyChanged("SeasonFruits");
                InvokePropertyChanged("SeasonVegetables");
                InvokePropertyChanged("SeasonNuts");
            }
        }

        public IEnumerable<Ingredient> SeasonFruits
        {
            get
            {
                return mSortAscending ? seasonFruits : seasonFruits.Reverse();
            }
        }

        public IEnumerable<Ingredient> SeasonVegetables
        {
            get
            {
                return mSortAscending ? seasonVegetables : seasonVegetables.Reverse();
            }
        }

        public IEnumerable<Ingredient> SeasonNuts
        {
            get
            {
                return mSortAscending ? seasonNuts : seasonNuts.Reverse();
            }
        }

        public SaisonCalendarViewModel()
        {
            LoadDataFromBaseList();
        }

        public void LoadDataFromBaseList()
        {
            seasonFruits = App.AvailableIngredients.Where(x => x.IngredientCategory == IngredientCategory.Fruits &&
                             x.Seasons?.Count > 0).OrderBy(x => x.Name);
            seasonVegetables = App.AvailableIngredients.Where(x => x.IngredientCategory == IngredientCategory.Vegetables &&
                            x.Seasons?.Count > 0).OrderBy(x => x.Name);
            seasonNuts = App.AvailableIngredients.Where(x => x.IngredientCategory == IngredientCategory.Nuts &&
                            x.Seasons?.Count > 0).OrderBy(x => x.Name);
        }
    }
}

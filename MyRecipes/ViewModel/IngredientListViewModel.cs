using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel
{
    class IngredientListViewModel : ViewModelBase
    {
        private string mNewIngredientName;
        private string mIngredientListRaw;
        private bool mSortAscending = true;

        private string mIngredientSearch;
        private IngredientCategory mSelectedCategory = IngredientCategory.Unset;
        private bool mShowFiltered;
        private List<Ingredient> mFilteredIngredients = new List<Ingredient>();

        public List<Ingredient> AvailableIngredients
        {
            get
            {
                var data = mShowFiltered ? mFilteredIngredients.OrderBy(x => x.Name) :
                                                            App.AvailableIngredients.OrderBy(x => x.Name);

                return mSortAscending ? data.ToList() : data.Reverse().ToList();
            }
        }

        public bool SortAscending
        {
            get => mSortAscending;
            set
            {
                mSortAscending = value;
                InvokePropertyChanged();
                ForceUpdateList();
            }
        }

        public string IngredientSearch
        {
            get => mIngredientSearch;
            set
            {
                mIngredientSearch = value;
                InvokePropertyChanged();
            }
        }

        public IngredientCategory SelectedCategory
        {
            get => mSelectedCategory;
            set
            {
                mSelectedCategory = value;
                InvokePropertyChanged();
            }
        }

        public bool ShowFiltered
        {
            get => mShowFiltered;
            set
            {
                mShowFiltered = value;
                InvokePropertyChanged();
                InvokePropertyChanged("AvailableIngredients");
                if (!value)
                {
                    mFilteredIngredients.Clear();
                }
            }
        }

        public string NewIngredientName
        {
            get => mNewIngredientName;
            set
            {
                mNewIngredientName = value;
                InvokePropertyChanged();
                InvokePropertyChanged("AddIngredientEnabled");
            }
        }

        public bool AddIngredientEnabled => !string.IsNullOrWhiteSpace(mNewIngredientName);

        public string IngredientListRaw
        {
            get => mIngredientListRaw;
            set
            {
                mIngredientListRaw = value;
                InvokePropertyChanged();
                InvokePropertyChanged("AddMultipleEnabled");
            }
        }

        public bool AddMultipleEnabled => !string.IsNullOrWhiteSpace(mIngredientListRaw);

        public void SetFilteredIngredients(List<Ingredient> filteredIngredients)
        {
            mFilteredIngredients.Clear();
            mFilteredIngredients.AddRange(filteredIngredients);
            ShowFiltered = true;
        }

        public void ForceUpdateList()
        {
            InvokePropertyChanged("AvailableIngredients");
        }
    }
}

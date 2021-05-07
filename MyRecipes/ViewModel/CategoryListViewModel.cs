using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel
{
    class CategoryListViewModel : ViewModelBase
    {
        private string mNewCategoryName;
        private string mCategoryListRaw;
        private bool mSortAscending = true;

        public List<Category> AvailableCategories => App.AvailableCategories.OrderBy(x => x.Name).ToList();

        public string NewCategoryName
        {
            get => mNewCategoryName;
            set
            {
                mNewCategoryName = value;
                InvokePropertyChanged();
                InvokePropertyChanged("AddCategoryEnabled");
            }
        }

        public bool AddCategoryEnabled => !string.IsNullOrWhiteSpace(mNewCategoryName);

        public string CategoryListRaw
        {
            get => mCategoryListRaw;
            set
            {
                mCategoryListRaw = value;
                InvokePropertyChanged();
                InvokePropertyChanged("AddMultipleEnabled");
            }
        }

        public bool AddMultipleEnabled => !string.IsNullOrWhiteSpace(mCategoryListRaw);

        public void ForceUpdateList()
        {
            InvokePropertyChanged("AvailableCategories");
        }
    }
}

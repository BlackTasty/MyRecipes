using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    class CategoryListDesignViewModel : CategoryListViewModel
    {
        public CategoryListDesignViewModel()
        {
            AvailableCategories.AddRange(TestData.GetTestCategories());
        }
    }
}

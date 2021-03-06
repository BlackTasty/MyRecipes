using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;

namespace MyRecipes.ViewModel
{
    class HomeViewModel : ViewModelBase
    {
        public VeryObservableCollection<Recipe> History
        {
            get => App.History;
        }
    }
}

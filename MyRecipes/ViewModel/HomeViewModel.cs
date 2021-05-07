using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel
{
    class HomeViewModel : ViewModelBase
    {
        private VeryObservableCollection<Recipe> mHistory = new VeryObservableCollection<Recipe>("History");

        public VeryObservableCollection<Recipe> History
        {
            get => App.History;
        }
    }
}

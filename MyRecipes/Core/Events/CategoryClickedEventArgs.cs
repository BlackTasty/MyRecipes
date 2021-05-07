using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Events
{
    public class CategoryClickedEventArgs : EventArgs
    {
        private Category category;

        public Category Category => category;

        public CategoryClickedEventArgs(Category category)
        {
            this.category = category;
        }
    }
}

using MyRecipes.Core.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.SeasonCalendar
{
    public class SeasonConfiguratorOpeningEventArgs : EventArgs
    {
        private Ingredient ingredient;

        public Ingredient Ingredient => ingredient;

        public SeasonConfiguratorOpeningEventArgs(Ingredient ingredient)
        {
            this.ingredient = ingredient;
        }
    }
}

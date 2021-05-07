using MyRecipes.Core.SeasonCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    class SeasonIngredientConfiguratorDesignViewModel : SeasonIngredientConfiguratorViewModel
    {
        public SeasonIngredientConfiguratorDesignViewModel()
        {
            Ingredient = TestData.GetTestIngredient("Apfel");
            Ingredient.Seasons.Add(new Season(SeasonMonth.April, SeasonMonth.July, WareOriginType.Warehouse, false));
            Ingredient.Seasons.Add(new Season(SeasonMonth.March, SeasonMonth.September, WareOriginType.Fresh, false));
            Ingredient.Seasons.Add(new Season(SeasonMonth.January, SeasonMonth.July, WareOriginType.Warehouse, false));
            Ingredient.Seasons.Add(new Season(SeasonMonth.January, SeasonMonth.July, WareOriginType.Fresh, false));
            Ingredient.Seasons.Add(new Season(SeasonMonth.March, SeasonMonth.December, WareOriginType.Warehouse, false));
            Ingredient.Seasons.Add(new Season(SeasonMonth.March, SeasonMonth.December, WareOriginType.Fresh, false));
            Ingredient.Seasons.Add(new Season(SeasonMonth.January, SeasonMonth.December, WareOriginType.Warehouse, true));
            Ingredient.Seasons.Add(new Season(SeasonMonth.January, SeasonMonth.December, WareOriginType.Fresh, false));
        }
    }
}

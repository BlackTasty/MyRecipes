using MyRecipes.Core.SeasonCalendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.DesignTime
{
    class SaisonCalendarDesignViewModel : SaisonCalendarViewModel
    {
        public SaisonCalendarDesignViewModel()
        {
            List<Season> appleSeasons = new List<Season>()
            {
                new Season(SeasonMonth.January, SeasonMonth.December, WareOriginType.Warehouse, true),
                new Season(SeasonMonth.August, SeasonMonth.October, WareOriginType.Fresh, false)
            };

            List<Season> walnutSeasons = new List<Season>()
            {
                new Season(SeasonMonth.November, SeasonMonth.January, WareOriginType.Warehouse, false),
                new Season(SeasonMonth.September, SeasonMonth.October, WareOriginType.Fresh, false)
            };

            App.AvailableIngredients.Add(TestData.GetTestIngredient("Walnus", walnutSeasons));
            App.AvailableIngredients.Add(TestData.GetTestIngredient("Apfel", appleSeasons));
        }
    }
}

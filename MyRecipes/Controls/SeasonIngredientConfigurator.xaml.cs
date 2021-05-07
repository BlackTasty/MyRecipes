using MyRecipes.Core.Recipes;
using MyRecipes.Core.SeasonCalendar;
using MyRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyRecipes.Controls
{
    /// <summary>
    /// Interaction logic for SeasonIngredientConfigurator.xaml
    /// </summary>
    public partial class SeasonIngredientConfigurator : DockPanel
    {
        public SeasonIngredientConfigurator(Ingredient ingredient)
        {
            InitializeComponent();
            (DataContext as SeasonIngredientConfiguratorViewModel).Ingredient = ingredient;
        }

        private void AddSeason_Click(object sender, RoutedEventArgs e)
        {
            SeasonIngredientConfiguratorViewModel vm = DataContext as SeasonIngredientConfiguratorViewModel;

            vm.Ingredient.Seasons.Add(new Season(vm.RangeStart, vm.RangeEnd, vm.WareOriginType, vm.WholeYear));
            vm.WareOriginType = WareOriginType.Unset;
        }

        private void RemoveSaison_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Season season)
            {
                SeasonIngredientConfiguratorViewModel vm = DataContext as SeasonIngredientConfiguratorViewModel;
                vm.Ingredient.Seasons.Remove(season);
                
            }
        }
    }
}

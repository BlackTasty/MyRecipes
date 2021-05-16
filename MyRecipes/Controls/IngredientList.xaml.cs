using ControlzEx.Standard;
using MyRecipes.Core.Enum;
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
    /// Interaction logic for IngredientList.xaml
    /// </summary>
    public partial class IngredientList : DockPanel
    {
        public event EventHandler<SeasonConfiguratorOpeningEventArgs> SeasonConfiguratorOpening;

        public IngredientList()
        {
            InitializeComponent();
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            IngredientListViewModel vm = DataContext as IngredientListViewModel;
            if (!vm.AvailableIngredients.Any(x => x.Name.Equals(vm.NewIngredientName, StringComparison.InvariantCultureIgnoreCase)))
            {
                App.AvailableIngredients.Add(new Ingredient(vm.NewIngredientName));
                vm.NewIngredientName = "";
                vm.ForceUpdateList();
            }
        }

        private void AddIngredients_Click(object sender, RoutedEventArgs e)
        {
            IngredientListViewModel vm = DataContext as IngredientListViewModel;

            string[] ingredients = vm.IngredientListRaw.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

            foreach (string ingredient in ingredients)
            {
                if (!vm.AvailableIngredients.Any(x => x.Name.Equals(ingredient, StringComparison.InvariantCultureIgnoreCase)))
                {
                    App.AvailableIngredients.Add(new Ingredient(ingredient));
                }
            }

            vm.IngredientListRaw = "";
            vm.ForceUpdateList();
        }

        private void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Ingredient ingredient)
            {
                IngredientListViewModel vm = DataContext as IngredientListViewModel;
                App.AvailableIngredients.Remove(ingredient);
                vm.ForceUpdateList();
            }
        }

        private void AbortAddMultiple_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as IngredientListViewModel).IngredientListRaw = "";
        }

        private void OpenSeasonConfigurator_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Ingredient ingredient)
            {
                OnSeasonConfiguratorOpening(new SeasonConfiguratorOpeningEventArgs(ingredient));
            }
        }

        protected virtual void OnSeasonConfiguratorOpening(SeasonConfiguratorOpeningEventArgs e)
        {
            SeasonConfiguratorOpening?.Invoke(this, e);
        }

        private void ToggleSearch_Manage_Click(object sender, RoutedEventArgs e)
        {
            topCardTransitioner.SelectedIndex = topCardTransitioner.SelectedIndex == 0 ? 1 : 0;
            if (topCardTransitioner.SelectedIndex == 0)
            {
                IngredientListViewModel vm = DataContext as IngredientListViewModel;
                vm.ShowFiltered = false;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            IngredientListViewModel vm = DataContext as IngredientListViewModel;
            List<Ingredient> filteredIngredients = new List<Ingredient>();

            foreach (Ingredient ingredient in App.AvailableIngredients)
            {
                if (vm.SelectedCategory != IngredientCategory.All)
                {
                    if (vm.SelectedCategory == ingredient.IngredientCategory)
                    {
                        if (!string.IsNullOrWhiteSpace(vm.IngredientSearch))
                        {
                            if (ingredient.Name.ToLower().Contains(vm.IngredientSearch.ToLower()))
                            {
                                filteredIngredients.Add(ingredient);
                            }
                        }
                        else
                        {
                            filteredIngredients.Add(ingredient);
                        }
                    }
                }
                else if (!string.IsNullOrWhiteSpace(vm.IngredientSearch))
                {
                    if (ingredient.Name.ToLower().Contains(vm.IngredientSearch.ToLower()))
                    {
                        filteredIngredients.Add(ingredient);
                    }
                }
                else
                {
                    filteredIngredients.Add(ingredient);
                }
            }

            vm.SetFilteredIngredients(filteredIngredients);
        }

        private void IngredientCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IngredientListViewModel vm = DataContext as IngredientListViewModel;
         
            if (sender is ComboBox comboBox && comboBox.SelectedItem is IngredientCategory category)
            {
                Ingredient ingredient = comboBox.DataContext as Ingredient;
                switch (category)
                {
                    case IngredientCategory.Alcohol:
                    case IngredientCategory.Liquid:
                    case IngredientCategory.Oil:
                    case IngredientCategory.Sauces:
                        ingredient.MeasurementType = MeasurementType.Milliliters;
                        break;
                    case IngredientCategory.Flour:
                    case IngredientCategory.Rice:
                    case IngredientCategory.Bread:
                        ingredient.MeasurementType = MeasurementType.Gram;
                        break;
                }
            }
        }
    }
}

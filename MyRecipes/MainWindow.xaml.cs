using MahApps.Metro.Controls;
using MyRecipes.Controls;
using MyRecipes.Core.Sidebar;
using MyRecipes.ViewModel;
using MyRecipes.ViewModel.DesignTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyRecipes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void sidebarMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            if (e.AddedItems.Count > 0 && !(e.AddedItems[0] as SidebarEntry).HasChildren)
            {
                MenuToggleButton.IsChecked = false;
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void BackToIngredientsList_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).SelectedSidebarIndex = 2;
        }

        private void BackToRecipes_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).SelectedSidebarIndex = 1;
        }

        private void OpenSaisonCalendar_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).SelectedSidebarIndex = 3;
        }

        private void ToggleShoppingList_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = DataContext as MainViewModel;
            vm.ShowShoppingList = !vm.ShowShoppingList;
        }

        private void ClearShoppingList_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = DataContext as MainViewModel;
            vm.ShoppingList.Clear();
            vm.RefreshShoppingListBinding();
            vm.ShowShoppingList = false;
        }

        private void ExportShoppingList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).SaveRecipe();
        }

        private void EditRecipe_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel vm = DataContext as MainViewModel;
            if (vm.Content is RecipeView recipeView && recipeView.DataContext is RecipeViewViewModel recipeVm)
            {
                vm.OpenRecipe(recipeVm.Recipe, true);
            }
        }
    }
}

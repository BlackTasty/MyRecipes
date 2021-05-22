using MaterialDesignThemes.Wpf;
using MyRecipes.Core;
using MyRecipes.Core.Events;
using MyRecipes.Core.Recipes;
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
    /// Interaction logic for RecipeList.xaml
    /// </summary>
    public partial class RecipeList : DockPanel
    {
        public event EventHandler<RecipeOpeningEventArgs> RecipeEditing;
        public event EventHandler<RecipeOpeningEventArgs> RecipeOpening;
        public event EventHandler<RecipeOpeningEventArgs> AddingToShoppingList;

        public RecipeList()
        {
            InitializeComponent();
        }

        public void SetCategoryFilter(Category category)
        {
            RecipeListViewModel vm = DataContext as RecipeListViewModel;
            if (!vm.SearchByCategory)
            {
                vm.SearchByCategory = true;
            }
            vm.SelectedCategory = vm.AvailableCategories.FirstOrDefault(x => x.Name == category.Name);
            ExecuteSearch();
        }

        public void ReloadList()
        {
            ExecuteSearch();
        }

        private void Chip_DeleteClick(object sender, RoutedEventArgs e)
        {
            if (sender is Chip chip && chip.DataContext is FilterObject ingredient)
            {
                RecipeListViewModel vm = DataContext as RecipeListViewModel;
                vm.SelectedIngredients.Remove(ingredient);
                ExecuteSearch();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ExecuteSearch();
        }

        private void ExecuteSearch()
        {
            RecipeListViewModel vm = DataContext as RecipeListViewModel;
            List<Recipe> filteredRecipes = new List<Recipe>();
            bool isSearchSet = false;

            if (vm.SearchByCategory)
            {
                if (vm.SelectedCategory != null)
                {
                    foreach (Recipe recipe in App.AvailableRecipes)
                    {
                        if (recipe.Categories.Any(x => x.Name == vm.SelectedCategory.Name))
                        {
                            filteredRecipes.Add(recipe);
                            continue;
                        }
                    }
                    isSearchSet = true;
                }
                else
                {
                    vm.ShowFiltered = false;
                }
            }
            else if (vm.SearchByIngredient)
            {
                if (vm.SelectedIngredients.Count > 0)
                {
                    foreach (Recipe recipe in App.AvailableRecipes)
                    {
                        bool matchesIngredientSearch = true;
                        foreach (FilterObject ingredient in vm.SelectedIngredients)
                        {
                            if (!recipe.Ingredients.Any(x => x.Ingredient.Name == ingredient.Name))
                            {
                                matchesIngredientSearch = false;
                                break;
                            }
                        }

                        if (matchesIngredientSearch)
                        {
                            filteredRecipes.Add(recipe);
                        }
                    }
                    isSearchSet = true;
                }
                else
                {
                    vm.ShowFiltered = false;
                }
            }
            else if (vm.SearchByRecipe)
            {
                if (!string.IsNullOrWhiteSpace(vm.SearchRecipeName))
                {
                    filteredRecipes.AddRange(App.AvailableRecipes.Where(x => x.Name.Contains(vm.SearchRecipeName)));
                    isSearchSet = true;
                }
                else
                {
                    vm.ShowFiltered = false;
                }
            }

            if (isSearchSet)
            {
                vm.SetFilteredRecipes(filteredRecipes);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox ingredients && ingredients.SelectedItem is FilterObject ingredient)
            {
                RecipeListViewModel vm = DataContext as RecipeListViewModel;
                if (!vm.SelectedIngredients.Contains(ingredient))
                {
                    vm.SelectedIngredients.Add(ingredient);
                }
                ingredients.SelectedItem = null;
                ExecuteSearch();
            }
        }

        private void CreateRecipe_Click(object sender, RoutedEventArgs e)
        {
            RecipeListViewModel vm = DataContext as RecipeListViewModel;
            OnRecipeEditing(new RecipeOpeningEventArgs(vm.NewRecipeName));
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Chip chip && chip.DataContext is Category category)
            {
                SetCategoryFilter(category);
            }
        }

        protected virtual void OnRecipeEditing(RecipeOpeningEventArgs e)
        {
            RecipeEditing?.Invoke(this, e);
        }

        protected virtual void OnRecipeOpening(RecipeOpeningEventArgs e)
        {
            RecipeOpening?.Invoke(this, e);
        }

        private void EditRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Recipe recipe)
            {
                OnRecipeEditing(new RecipeOpeningEventArgs(recipe));
            }
        }

        private void Recipe_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && item.DataContext is Recipe recipe)
            {
                OnRecipeOpening(new RecipeOpeningEventArgs(recipe));
            }
        }

        private void border_addRecipe_MouseEnter(object sender, MouseEventArgs e)
        {
            btn_addRecipe.IsChecked = true;
        }

        private void border_addRecipe_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_addRecipe.IsChecked = false;
        }

        private void RemoveRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Recipe recipe)
            {
                RecipeListViewModel vm = DataContext as RecipeListViewModel;
                vm.SelectedRecipeForDeletion = recipe;
            }
        }

        private void ConfirmRemoveRecipe_Click(object sender, RoutedEventArgs e)
        {
            RecipeListViewModel vm = DataContext as RecipeListViewModel;
            vm.SelectedRecipeForDeletion.Delete();
            App.AvailableRecipes.Remove(vm.SelectedRecipeForDeletion);
            App.LoadHistory();
            ReloadList();
        }

        private void ClearCategorySelection_Click(object sender, RoutedEventArgs e)
        {
            RecipeListViewModel vm = DataContext as RecipeListViewModel;
            vm.SelectedCategory = null;
            //ExecuteSearch();
        }

        private void FilterCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ExecuteSearch();
        }

        private void SearchRecipeName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ExecuteSearch();
            }
        }

        private void AddToShoppingList_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Recipe recipe)
            {
                OnAddingToShoppingList(new RecipeOpeningEventArgs(recipe));
            }
        }

        protected virtual void OnAddingToShoppingList(RecipeOpeningEventArgs e)
        {
            AddingToShoppingList?.Invoke(this, e);
        }
    }
}

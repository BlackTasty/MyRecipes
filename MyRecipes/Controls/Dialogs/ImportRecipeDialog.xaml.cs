using MahApps.Metro.Controls;
using MyRecipes.Core.Export;
using MyRecipes.Core.Recipes;
using MyRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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
using Tasty.MaterialDesign.FilePicker;
using Tasty.MaterialDesign.FilePicker.Core;

namespace MyRecipes.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for ImportRecipeDialog.xaml
    /// </summary>
    public partial class ImportRecipeDialog : DockPanel
    {
        public ImportRecipeDialog()
        {
            InitializeComponent();
            (DataContext as ImportRecipeViewModel).RecipesChanged += Recipes_ObjectIsSelectedChanged;
            ShowFilePicker();
        }

        /*public ImportRecipeDialog(string filePath) : this()
        {
            (DataContext as ImportRecipeViewModel).FilePath = filePath;
        }*/

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            ImportRecipeViewModel vm = DataContext as ImportRecipeViewModel;

            foreach (ExportObject<Recipe> importedRecipe in vm.Recipes.ExportObjects.Where(x => x.IsSelected))
            {
                if (importedRecipe.Data is Recipe recipe)
                {
                    // Iterate through recipe ingredients and scan user library for match.
                    // if match found, replace current with matching ingredient
                    foreach (RecipeIngredient ingredient in recipe.Ingredients)
                    {
                        Ingredient existing = App.AvailableIngredients
                            .FirstOrDefault(x => x.Name.Equals(ingredient.Ingredient.Name, 
                                                    StringComparison.InvariantCultureIgnoreCase));
                        if (existing != null)
                        {
                            ingredient.Ingredient = existing;
                            break;
                        }
                    }

                    // Iterate through recipe categories and scan user library for match.
                    // if match found, replace current with matching category
                    for (int i = 0; i < recipe.Categories.Count; i++)
                    {
                        Category category = recipe.Categories[i];
                        Category existing = App.AvailableCategories
                            .FirstOrDefault(x => x.Name.Equals(category.Name, 
                                                    StringComparison.InvariantCultureIgnoreCase));
                        if (existing != null)
                        {
                            recipe.Categories[i] = existing;
                        }
                    }

                    // Iterate through recipe ingredients and check if not existing in user library
                    foreach (RecipeIngredient ingredient in recipe.Ingredients)
                    {
                        if (!App.AvailableIngredients
                                .Any(x => x.Guid.Equals(ingredient.Ingredient.Guid, 
                                    StringComparison.InvariantCultureIgnoreCase)))
                        {
                            App.AvailableIngredients.Add(ingredient.Ingredient);
                        }
                    }

                    // Iterate through recipe categories and check if not existing in user library
                    foreach (Category category in recipe.Categories)
                    {
                        if (!App.AvailableCategories
                                .Any(x => x.Guid.Equals(category.Guid,
                                    StringComparison.InvariantCultureIgnoreCase)))
                        {
                            App.AvailableCategories.Add(category);
                        }
                    }

                    string recipeName = recipe.Name;
                    if (App.AvailableRecipes.Any(x => x.Name.Equals(recipeName)))
                    {
                        recipeName = recipeName + " - " + DateTime.Now.ToString("ddMMyyyy HHmmss");
                    }

                    recipe.Name = recipeName;
                    App.AvailableRecipes.Add(recipe);
                }
            }

            App.SaveCookingData();
            App.LoadCookingData();

            App.SaveRecipes();
            App.LoadRecipes();
            App.LoadHistory();
        }

        private void ShowFilePicker()
        {
            FilePicker filePicker = new FilePicker()
            {
                Title = "Rezept-Datei zum Importieren wählen",
                Filter = "Rezept-Datei|*.recps",
                InitialDirectory = (DataContext as ImportRecipeViewModel).FilePath
            };
            filePicker.DialogClosed += FilePicker_DialogClosed;

            FilePicker.ShowDialog(filePicker, new MetroWindow());
        }

        private void OpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            ShowFilePicker();
        }

        private void FilePicker_DialogClosed(object sender, FilePickerClosedEventArgs e)
        {
            if (e.DialogResult == MessageBoxResult.OK)
            {
                (DataContext as ImportRecipeViewModel).FilePath = e.FilePath;
                txt_path.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            }
        }

        private void Recipes_ObjectIsSelectedChanged(object sender, EventArgs e)
        {
            ImportRecipeViewModel vm = DataContext as ImportRecipeViewModel;
            list_recipes.ItemsSource = vm.Recipes.ExportObjects;
            list_recipes.Items.Refresh();
            vm.RefreshSelectedCount();
        }
    }
}

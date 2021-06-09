using MahApps.Metro.Controls;
using MyRecipes.Core.Export;
using MyRecipes.Core.Recipes;
using MyRecipes.ViewModel;
using MyRecipes.ViewModel.Communication;
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
using Tasty.MaterialDesign.FilePicker;
using Tasty.MaterialDesign.FilePicker.Core;
using Tasty.ViewModel.Communication;

namespace MyRecipes.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for ImportCookingDataDialog.xaml
    /// </summary>
    public partial class ImportCookingDataDialog : DockPanel
    {
        public ImportCookingDataDialog()
        {
            InitializeComponent();
            (DataContext as ImportCookingDataViewModel).DataChanged += Recipes_ObjectIsSelectedChanged;
            ShowFilePicker();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            ImportCookingDataViewModel vm = DataContext as ImportCookingDataViewModel;

            // Iterate through recipe ingredients and check if not existing in user library
            foreach (ExportObject<Ingredient> importedIngredient in vm.Ingredients.ExportObjects.Where(x => x.IsSelected))
            {
                if (importedIngredient.Data is Ingredient ingredient)
                {
                    if (!App.AvailableIngredients
                            .Any(x => x.Name.Equals(ingredient.Name,
                                StringComparison.InvariantCultureIgnoreCase)))
                    {
                        App.AvailableIngredients.Add(ingredient);
                    }
                }
            }

            // Iterate through recipe categories and check if not existing in user library
            foreach (ExportObject<Category> importedCategory in vm.Categories.ExportObjects.Where(x => x.IsSelected))
            {
                if (importedCategory.Data is Category category)
                {
                    if (!App.AvailableCategories
                            .Any(x => x.Name.Equals(category.Name,
                                StringComparison.InvariantCultureIgnoreCase)))
                    {
                        App.AvailableCategories.Add(category);
                    }
                }
            }

            App.SaveCookingData();
            App.LoadCookingData();

            Mediator.Instance.NotifyColleagues(ViewModelMessage.CookingDataImported, null);
        }

        private void ShowFilePicker()
        {
            FilePicker filePicker = new FilePicker()
            {
                Title = "Kochdaten-Datei zum Importieren wählen",
                Filter = "Kochdaten-Datei|*.como",
                InitialDirectory = (DataContext as ImportCookingDataViewModel).FilePath
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
                (DataContext as ImportCookingDataViewModel).FilePath = e.FilePath;
                txt_path.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            }
        }

        private void Recipes_ObjectIsSelectedChanged(object sender, EventArgs e)
        {
            ImportCookingDataViewModel vm = DataContext as ImportCookingDataViewModel;
            list_categories.ItemsSource = vm.Categories.ExportObjects;
            list_categories.Items.Refresh();
            list_ingredients.ItemsSource = vm.Ingredients.ExportObjects;
            list_ingredients.Items.Refresh();
            vm.RefreshSelectedCount();
        }
    }
}

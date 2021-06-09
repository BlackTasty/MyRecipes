using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
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
using System.Windows.Shapes;
using Tasty.MaterialDesign.FilePicker;
using Tasty.MaterialDesign.FilePicker.Core;

namespace MyRecipes.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for ExportRecipeDialog.xaml
    /// </summary>
    public partial class ExportRecipeDialog : DockPanel
    {
        public ExportRecipeDialog()
        {
            InitializeComponent();
            (DataContext as ExportRecipeViewModel).Recipes.ObjectIsSelectedChanged += Recipes_ObjectIsSelectedChanged;
        }

        public ExportRecipeDialog(Recipe recipe) : this()
        {
            SelectRecipe(recipe);
        }

        public ExportRecipeDialog(List<Recipe> recipes) : this()
        {
            SelectRecipes(recipes);
        }

        public void SelectRecipe(Recipe recipe)
        {
            ExportRecipeViewModel vm = DataContext as ExportRecipeViewModel;
            vm.Recipes.SelectObjectForExport(recipe);
            vm.RefreshAllSelected();
        }

        public void SelectRecipes(List<Recipe> recipes)
        {
            ExportRecipeViewModel vm = DataContext as ExportRecipeViewModel;
            vm.Recipes.SelectObjectsForExport(recipes);
            vm.RefreshAllSelected();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            ExportRecipeViewModel vm = DataContext as ExportRecipeViewModel;
            vm.RunExport();
        }

        private void OpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            FilePicker filePicker = new FilePicker()
            {
                Title = "Ziel-Ordner für Rezept Datei angeben",
                //Filter = "Rezept-Datei|*.recps",
                IsFolderSelect = true
            };
            filePicker.DialogClosed += FilePicker_DialogClosed;

            FilePicker.ShowDialog(filePicker, new MetroWindow());
        }

        private void FilePicker_DialogClosed(object sender, FilePickerClosedEventArgs e)
        {
            if (e.DialogResult == MessageBoxResult.OK)
            {
                (DataContext as ExportRecipeViewModel).ExportPath = e.FilePath;
                txt_path.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            }
        }

        private void Recipes_ObjectIsSelectedChanged(object sender, EventArgs e)
        {
            list_recipes.Items.Refresh();
            (DataContext as ExportRecipeViewModel).RefreshSelectedCount();
        }
    }
}

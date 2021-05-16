using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;
using MaterialDesignThemes.Wpf;
using MyRecipes.Core.Enum;
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
    /// Interaction logic for CreateEditRecipe.xaml
    /// </summary>
    public partial class CreateEditRecipe : Grid
    {
        public event EventHandler<EventArgs> Finished;

        public CreateEditRecipe()
        {
            InitializeComponent();
            dialog.Filters = FileFilterHelper.ParseFileFilters("Bilder|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Alle Dateien|*.*");
            dialog.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public CreateEditRecipe(Recipe recipe, bool isEdit = true) : this()
        {
            CreateEditRecipeViewModel vm = DataContext as CreateEditRecipeViewModel;
            vm.Recipe = recipe;
            vm.IsEdit = isEdit;
        }

        public CreateEditRecipe(string name) : this(new Recipe(name), false)
        {
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            CreateEditRecipeViewModel vm = DataContext as CreateEditRecipeViewModel;
            vm.Recipe.Ingredients.Add(new RecipeIngredient(vm.AvailableIngredients[vm.SelectedIngredientIndex],
                vm.IngredientAmount, (MeasurementType)vm.SelectedMeasurementTypeIndex));
        }

        private void AddEditPreparationStep_Click(object sender, RoutedEventArgs e)
        {
            CreateEditRecipeViewModel vm = DataContext as CreateEditRecipeViewModel;

            if (!vm.IsEditPreparationStep)
            {
                vm.Recipe.PreparationSteps.Add(vm.NewPreparationStepText);
                vm.NewPreparationStepText = "";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            CreateEditRecipeViewModel vm = DataContext as CreateEditRecipeViewModel;
            if (!vm.IsEdit)
            {
                App.AvailableRecipes.Add(vm.Recipe);
            }
            vm.Recipe.Save(App.Settings.RecipeDirectory);
            OnFinished(EventArgs.Empty);
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            OnFinished(EventArgs.Empty);
        }

        private void RemovePreparationStep_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag.ToString(), out int index))
            {
                CreateEditRecipeViewModel vm = DataContext as CreateEditRecipeViewModel;
                vm.Recipe.PreparationSteps.RemoveAt(index);
                System.ComponentModel.ICollectionView view = CollectionViewSource.GetDefaultView(preparationStepsList.ItemsSource);
                view.Refresh();
            }
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            /*OpenFileDialog dialog = new OpenFileDialog()
            {
                SwitchPathPartsAsButtonsEnabled = true
            };
            dialog.Filters = FileFilterHelper.ParseFileFilters("Bilder|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Alle Dateien|*.*");
            dialog.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);*/

            (DataContext as CreateEditRecipeViewModel).ShowOpenFileDialog = true;
        }

        private void dialog_Cancel(object sender, RoutedEventArgs e)
        {
            (DataContext as CreateEditRecipeViewModel).ShowOpenFileDialog = false;
        }

        private void dialog_FileSelected(object sender, RoutedEventArgs e)
        {
            CreateEditRecipeViewModel vm = DataContext as CreateEditRecipeViewModel;
            vm.Recipe.RecipeImage = new RecipeImage(dialog.CurrentFile);
            vm.ShowOpenFileDialog = false;
        }

        private void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is RecipeIngredient recipeIngredient)
            {
                CreateEditRecipeViewModel vm = DataContext as CreateEditRecipeViewModel;
                vm.Recipe.Ingredients.Remove(recipeIngredient);
            }
        }

        protected virtual void OnFinished(EventArgs e)
        {
            Finished?.Invoke(this, e);
        }

        private void Category_DeleteClick(object sender, RoutedEventArgs e)
        {
            if (sender is Chip chip && chip.DataContext is Category category)
            {
                CreateEditRecipeViewModel vm = DataContext as CreateEditRecipeViewModel;
                vm.Recipe.Categories.Remove(category);
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            CreateEditRecipeViewModel vm = DataContext as CreateEditRecipeViewModel;
            vm.Recipe.Categories.Add(vm.SelectedCategory);
            vm.SelectedCategory = null;
        }

        private async void EditPreparationStep_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag.ToString(), out int index))
            {
                CreateEditRecipeViewModel vm = DataContext as CreateEditRecipeViewModel;
                vm.IsEditPreparationStep = true;
                string preparationStepCurrent = vm.Recipe.PreparationSteps[index];
                vm.NewPreparationStepText = preparationStepCurrent;

                var result = await dialogHost.ShowDialog(dialog_preparationStep, 
                    delegate(object dialogSender, DialogOpenedEventArgs args)
                {
                });

                vm.Recipe.PreparationSteps[index] = vm.NewPreparationStepText;
                System.ComponentModel.ICollectionView view = CollectionViewSource.GetDefaultView(preparationStepsList.ItemsSource);
                view.Refresh();
                vm.NewPreparationStepText = "";
            }
        }
    }
}

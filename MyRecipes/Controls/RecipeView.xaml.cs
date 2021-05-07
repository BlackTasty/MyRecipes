using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for RecipeView.xaml
    /// </summary>
    public partial class RecipeView : DockPanel
    {
        public event EventHandler<CategoryClickedEventArgs> CategoryClicked;

        public RecipeView(Recipe recipe)
        {
            InitializeComponent();
            (DataContext as RecipeViewViewModel).Recipe = recipe;
            recipe.LastAccessDate = DateTime.Now;
            recipe.Save(recipe.FilePath);
            App.LoadHistory();
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Chip chip && chip.DataContext is Category category)
            {
                OnCategoryClicked(new CategoryClickedEventArgs(category));
            }
        }

        protected void OnCategoryClicked(CategoryClickedEventArgs e)
        {
            CategoryClicked?.Invoke(this, e);
        }
    }
}

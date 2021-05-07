using MyRecipes.Core.Events;
using MyRecipes.Core.Recipes;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : DockPanel
    {
        public event EventHandler<RecipeOpeningEventArgs> RecipeOpening;

        public Home()
        {
            InitializeComponent();
        }

        private void RefreshHistory_Click(object sender, RoutedEventArgs e)
        {
            App.LoadHistory();
        }

        private void ProjectHistoryItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && item.DataContext is Recipe recipe)
            {
                OnRecipeOpening(new RecipeOpeningEventArgs(recipe));
            }
        }

        protected virtual void OnRecipeOpening(RecipeOpeningEventArgs e)
        {
            RecipeOpening?.Invoke(this, e);
        }
    }
}

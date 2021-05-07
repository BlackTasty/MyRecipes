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
    /// Interaction logic for CategoryList.xaml
    /// </summary>
    public partial class CategoryList : DockPanel
    {
        public CategoryList()
        {
            InitializeComponent();
        }

        private void AddCategories_Click(object sender, RoutedEventArgs e)
        {
            CategoryListViewModel vm = DataContext as CategoryListViewModel;

            string[] categories = vm.CategoryListRaw.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

            foreach (string ingredient in categories)
            {
                if (!vm.AvailableCategories.Any(x => x.Name.Equals(ingredient, StringComparison.InvariantCultureIgnoreCase)))
                {
                    App.AvailableCategories.Add(new Category(ingredient));
                }
            }

            vm.CategoryListRaw = "";
            vm.ForceUpdateList();
        }

        private void AbortAddMultiple_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as CategoryListViewModel).CategoryListRaw = "";
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            CategoryListViewModel vm = DataContext as CategoryListViewModel;
            if (!vm.AvailableCategories.Any(x => x.Name.Equals(vm.NewCategoryName, StringComparison.InvariantCultureIgnoreCase)))
            {
                App.AvailableCategories.Add(new Category(vm.NewCategoryName));
                vm.NewCategoryName = "";
                vm.ForceUpdateList();
            }
        }

        private void RemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Category category)
            {
                CategoryListViewModel vm = DataContext as CategoryListViewModel;
                App.AvailableCategories.Remove(category);
                vm.ForceUpdateList();
            }
        }
    }
}

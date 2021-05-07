using MahApps.Metro.Controls;
using MyRecipes.Core.Recipes;
using MyRecipes.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyRecipes.Converter
{
    class ListIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index;

            if (parameter != null && int.TryParse(parameter.ToString(), out int paramIndex))
            {
                index = paramIndex;
            }
            else
            {
                index = 0;
            }

            if (value is VeryObservableCollection<RecipeImage> recipeImages)
            {
                if (index < recipeImages.Count)
                {
                    return recipeImages[index].Image;
                }
                else
                {
                    return Core.Utils.BitmapToBitmapImage(Properties.Resources.no_image);
                }
            }
            else if (value is IList list)
            {
                if (index < list.Count)
                {
                    return list[index];
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

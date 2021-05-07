using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MyRecipes.Converter
{
    class SeasonBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value.ToString(), out int valueInt))
            {
                switch (valueInt)
                {
                    case 1:
                        return new SolidColorBrush(Color.FromArgb(128, 194, 238, 128));
                        return App.Current.TryFindResource("SeasonWarehouseBackground");
                    case 2:
                        return new SolidColorBrush(Color.FromArgb(128, 104, 233, 56));
                        return App.Current.TryFindResource("SeasonFreshBackground");
                    default:
                        return new SolidColorBrush(Color.FromRgb(48, 48, 48));
                        return App.Current.TryFindResource("MaterialDesignPaper");
                }
            }
            else
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value.ToString(), out int valueInt))
            {
                switch (valueInt)
                {
                    case 1:
                        return App.Current.TryFindResource("SeasonWarehouseBackground");
                    case 2:
                        return App.Current.TryFindResource("SeasonFreshBackground");
                    default:
                        return App.Current.TryFindResource("MaterialDesignPaper");
                }
            }
            else
            {
                return Binding.DoNothing;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyRecipes.Converter
{
    class MinutesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int minutesWhole)
            {
                int hours = (int)Math.Floor((double)minutesWhole / 60);
                int minutes = minutesWhole % 60;

                if (hours > 0)
                {
                    return string.Format("{0} h {1} min.", hours, minutes);
                }
                else
                {
                    return string.Format("{0} min.", minutes);
                }
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

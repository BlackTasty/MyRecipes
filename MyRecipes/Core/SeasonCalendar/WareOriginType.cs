using MyRecipes.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.SeasonCalendar
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum WareOriginType
    {
        [Description("Ungesetzt")]
        Unset = 0,
        [Description("Lagerware")]
        Warehouse = 1,
        [Description("Erntezeit")]
        Fresh = 2
    }
}

using MyRecipes.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Enum
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Unit
    {
        [Description("Pk")]
        Package, //e.g. Päckchen Vanillezucker
        [Description("ml")]
        Milliliters, //e.g. 500 ml Wasser
        [Description("g")]
        Gram, //e.g. 300 g Mehl
        [Description("Prise")]
        Pinch, //e.g. eine Prise Salz
        [Description("EL")]
        Tablespoon, //e.g. 4 Esslöffel Milch
        [Description("TL")]
        Teaspoon, //e.g. 3 Teelöffel Kurkuma
        [Description("Stk")]
        Piece //e.g. 3 Stk. Eier
    }
}

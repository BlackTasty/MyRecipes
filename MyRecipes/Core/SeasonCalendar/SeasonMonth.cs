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
    public enum SeasonMonth
    {
        [Description("Jänner")]
        January = 0,
        [Description("Februar")]
        February = 1,
        [Description("März")]
        March = 2,
        [Description("April")]
        April = 3,
        [Description("Mai")]
        May = 4,
        [Description("Juni")]
        June = 5,
        [Description("Juli")]
        July = 6,
        [Description("August")]
        August = 7,
        [Description("September")]
        September = 8,
        [Description("Oktober")]
        October = 9,
        [Description("November")]
        November = 10,
        [Description("Dezember")]
        December = 11,
    }
}

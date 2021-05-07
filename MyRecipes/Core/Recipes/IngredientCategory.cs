using MyRecipes.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Recipes
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum IngredientCategory
    {
        [Description("Alle")]
        All = -1,
        [Description("-")]
        Unset = 0,
        [Description("Gemüse")]
        Vegetables = 1,
        [Description("Obst")]
        Fruits = 2,
        [Description("Nüsse")]
        Nuts = 3,
        [Description("Fleisch (Geflügel)")]
        Pultry = 5,
        [Description("Fleisch (Rind)")]
        Beef = 6,
        [Description("Fleisch (Schwein)")]
        Pork = 7,
        [Description("Fleisch (Wild)")]
        Venison = 8,
        [Description("Fleisch (Lamm)")]
        Lamb = 9,
        [Description("Fleisch (gemischt)")]
        Meat = 10,
        [Description("Wurst")]
        Sausages = 11,
        [Description("Fisch")]
        Fish = 12,
        [Description("Pasta")]
        Pasta = 13,
        [Description("Reis")]
        Rice = 14,
        [Description("Milchprodukte")]
        Dairy = 15,
        [Description("Kräuter")]
        Herbs = 21,
        [Description("Gewürz/Süßes")]
        Spices = 22,
        [Description("Kerne/Samen")]
        Seeds = 23,
        [Description("Flüssigkeit")]
        Liquid = 30,
        [Description("Alkohol")]
        Alcohol = 31,
        [Description("Saucen")]
        Sauces = 35,
        [Description("Sahne/Cremes")]
        Creme = 36,
        [Description("Brot")]
        Bread = 40,
        [Description("Mehl")]
        Flour = 41,
        [Description("Pulver")]
        Powder = 42,
        [Description("Öle/Fett")]
        Oil = 43,
        [Description("Sonstige")]
        Other = 200
    }
}

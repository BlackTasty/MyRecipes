using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Observer
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ObservableProperty : Attribute
    {
        private object value;
        private string propertyName;

        public T ParseValue<T>(ref T defaultValue)
        {
            return value is T parsedValue ? parsedValue : defaultValue;
        }

        public ObservableProperty(object value, object observableClass, [CallerMemberName] string propertyName = null)
        {
            this.propertyName = propertyName;
            this.value = value;
        }
    }
}

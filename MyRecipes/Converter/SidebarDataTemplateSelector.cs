using MyRecipes.Core.Sidebar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace MyRecipes.Converter
{
    class SidebarDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EntryDataTemplate { get; set; }

        public DataTemplate SubEntryDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item.GetType() == typeof(SidebarEntry))
            {
                return EntryDataTemplate;
            }
            else if (item.GetType() == typeof(SidebarSubEntry))
            {
                return SubEntryDataTemplate;
            }

            return EntryDataTemplate;
        }
    }
}

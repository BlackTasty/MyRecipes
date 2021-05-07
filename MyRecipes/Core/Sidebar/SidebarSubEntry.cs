using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Sidebar
{
    class SidebarSubEntry : SidebarEntry
    {
        public SidebarEntry ParentEntry
        {
            get; private set;
        }

        public SidebarSubEntry(string sidebarText, PackIconKind icon, object content, int index,
            SidebarEntry parent) : this(sidebarText, icon, content, index, true, parent)
        {
        }

        public SidebarSubEntry(string sidebarText, PackIconKind icon, object content, int index,
            SidebarEntry parent, bool alwaysVisible) : this(sidebarText, icon, content, index, true, parent)
        {
            AlwaysVisible = alwaysVisible;
        }

        public SidebarSubEntry(string sidebarText, PackIconKind icon, object content, int index, bool isEnabled,
            SidebarEntry parent, bool alwaysVisible) : this(sidebarText, icon, content, index, isEnabled, parent)
        {
            AlwaysVisible = alwaysVisible;
        }

        public SidebarSubEntry(string sidebarText, PackIconKind icon, object content, int index, bool isEnabled,
            SidebarEntry parent) : base(sidebarText, icon, content, index, isEnabled)
        {
            ParentEntry = parent;
            IsVisible = System.Windows.Visibility.Collapsed;
        }
    }
}

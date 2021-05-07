using MaterialDesignThemes.Wpf;
using MyRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyRecipes.Core.Sidebar
{
    class SidebarEntry : ViewModelBase
    {
        private bool alwaysVisible;
        private Visibility mIsVisible = Visibility.Visible;

        public bool AlwaysVisible
        {
            get => alwaysVisible;
            set
            {
                alwaysVisible = value;
                InvokePropertyChanged("IsVisible");
            }
        }

        public string Name { get; private set; }

        public PackIconKind Icon { get; }

        public object Content { get; set; }

        public bool ContentIsCopy { get => (Content as FrameworkElement).Name.ToString() != Name; }

        public Visibility IsVisible
        {
            get => !alwaysVisible ? mIsVisible : Visibility.Visible;
            set
            {
                mIsVisible = value;
                InvokePropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get
            {
                /*if (Content == null)
                    return true;*/

                if (Content.GetType().IsSubclassOf(typeof(FrameworkElement)))
                {
                    return (Content as FrameworkElement).IsEnabled;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                //if (Content?.GetType().IsSubclassOf(typeof(FrameworkElement)) ?? false)
                //{
                //    (Content as FrameworkElement).IsEnabled = value;
                //}
                if (Content.GetType().IsSubclassOf(typeof(FrameworkElement)))
                {
                    (Content as FrameworkElement).IsEnabled = value;
                }
            }
        }

        public int Index { get; private set; }

        public List<SidebarSubEntry> SubEntries { get; private set; }

        public bool HasChildren { get => SubEntries?.Count > 0; }

        /// <summary>
        /// Creates a new item for the sidebar.
        /// </summary>
        /// <param name="sidebarText">The name of the resource inside the localisation files</param>
        /// <param name="icon">The icon to display in the list</param>
        /// <param name="content">The content which should be displayed in a content presenter</param>
        /// <param name="index">The index of this entry</param>
        public SidebarEntry(string sidebarText, PackIconKind icon, object content, int index)
        {
            Name = sidebarText;
            Icon = icon;

            Content = content;
            if (content != null)
            {
                (Content as FrameworkElement).Name = sidebarText;
            }
            Index = index;
        }

        /// <summary>
        /// Creates a new item for the sidebar.
        /// </summary>
        /// <param name="sidebarText">The name of the resource inside the localisation files</param>
        /// <param name="icon">The icon to display in the list</param>
        /// <param name="content">The content which should be displayed in a content presenter</param>
        /// <param name="index">The index of this entry</param>
        /// <param name="isEnabled">Toggles whether this item is enabled by default or not</param>
        public SidebarEntry(string sidebarText, PackIconKind icon, object content, int index, bool isEnabled)
            : this(sidebarText, icon, content, index)
        {
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// Creates a new item for the sidebar.
        /// </summary>
        /// <param name="sidebarText">The name of the resource inside the localisation files</param>
        /// <param name="icon">The icon to display in the list</param>
        /// <param name="content">The content which should be displayed in a content presenter</param>
        /// <param name="index">The index of this entry</param>
        /// <param name="isGhost">Determines if this entry is visible or not in the sidebar</param>
        public SidebarEntry(string sidebarText, PackIconKind icon, object content, int index, Visibility isGhost)
            : this(sidebarText, icon, content, index)
        {
            IsVisible = isGhost;
        }

        public void SetSubEntries(List<SidebarSubEntry> subEntries)
        {
            SubEntries = subEntries;
        }

        public void ToggleSubEntries(bool isOpen)
        {
            if (this.GetType() == typeof(SidebarEntry))
            {
                if (SubEntries?.Count > 0)
                {
                    foreach (SidebarSubEntry subEntry in SubEntries)
                    {
                        subEntry.IsVisible = isOpen ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
            else
            {
                (this as SidebarSubEntry).ParentEntry.ToggleSubEntries(isOpen);
            }
        }

        public bool MatchContentTag(string name)
        {
            return (Content as FrameworkElement).Name == name;
        }
    }
}

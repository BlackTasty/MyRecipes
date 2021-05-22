using ControlzEx.Standard;
using MyRecipes.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.Sorting
{
    class SortFactory<T>
    {
        private IEnumerable<T> list;
        private Type listType;

        private SortFactory(VeryObservableCollection<T> list)
        {
            this.list = list;
            listType = typeof(T);
        }

        public static SortFactory<T> BeginSort(VeryObservableCollection<T> list)
        {
            return new SortFactory<T>(list);
        }

        public SortFactory<T> SortWithRule(string propertyName, bool sortAscending)
        {
            PropertyInfo property = listType.GetProperty(propertyName);
            /*itch (propertyName)
            {
                case "Name":
                    list = sortAscending ? list.OrderBy(x => x.Name) : list.OrderByDescending(x => x.Name);
                    break;
                case "Description":
                    list = sortAscending ? list.OrderBy(x => x.Description) : list.OrderByDescending(x => x.Description);
                    break;
                case "LastModifyDate":
                    list = sortAscending ? list.OrderBy(x => x.LastModifyDate) : list.OrderByDescending(x => x.LastModifyDate);
                    break;
                case "LastAccessDate":
                    list = sortAscending ? list.OrderBy(x => x.LastAccessDate) : list.OrderByDescending(x => x.LastAccessDate);
                    break;
            }*/
            return this;
        }

        public SortFactory<T> Sort()
        {
            SortWithRule("Name", true);
            return this;
        }

        public IEnumerable<T> Execute()
        {
            return list;
        }
    }
}

using MyRecipes.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.Sorting
{
    class SortRule// : IOrderedEnumerable<IBaseData>
    {
        private string propertyName;
        private bool sortAscending;

        public SortRule(string propertyName, bool sortAscending)
        {
            this.sortAscending = sortAscending;
        }

        /*public IOrderedEnumerable<IBaseData> CreateOrderedEnumerable<TKey>(Func<IBaseData, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return sortAscending ? list.OrderBy(keySelector, comparer) : list.OrderByDescending(keySelector, comparer);
        }

        public IEnumerator<IBaseData> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }*/
    }
}

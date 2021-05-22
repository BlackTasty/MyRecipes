using MyRecipes.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.ViewModel.Sorting
{
    class SortChain
    {
        private IQueryable<IBaseData> ordered;

        private VeryObservableCollection<IBaseData> objects;
        private Queue<SortRule> sortChain;

        public static SortChain Create(VeryObservableCollection<IBaseData> objects)
        {
            return new SortChain(objects);
        }

        private SortChain(VeryObservableCollection<IBaseData> objects)
        {
            this.objects = objects;
        }

        /*public SortChain SortBy(SortRule sortParam)
        {

        }*/
    }
}

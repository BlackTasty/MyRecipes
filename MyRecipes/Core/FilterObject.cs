using MyRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;

namespace MyRecipes.Core
{
    class FilterObject : ViewModelBase
    {
        private string mName;
        private int mCounted;
        private IBaseData mData;

        public string Name
        {
            get => mName;
            set
            {
                mName = value;
                InvokePropertyChanged();
            }
        }

        public int Counted
        {
            get => mCounted;
            set
            {
                mCounted = value;
                InvokePropertyChanged();
            }
        }

        public IBaseData Data => mData;

        public FilterObject(string name)
        {
            Name = name;
            Counted = 0;
        }

        public FilterObject(IBaseData data) : this(data.Name)
        {
            mData = data;
            InvokePropertyChanged("Data");
        }

        public override string ToString()
        {
            return string.Format("{0} ({1} Rezepte)", Name, Counted);
        }
    }
}

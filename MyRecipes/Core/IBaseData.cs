using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel.Observer;

namespace MyRecipes.Core
{
    public interface IBaseData : IObservableClass
    {
        string Name { get; set; }

        string Description { get; set; }

        DateTime LastModifyDate { get; }

        DateTime LastAccessDate { get; }

        new IBaseData Copy();
    }
}

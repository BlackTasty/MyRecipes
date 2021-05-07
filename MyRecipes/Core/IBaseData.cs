using MyRecipes.Core.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core
{
    public interface IBaseData
    {
        ObserverManager ChangeManager { get; }

        string Guid { get; }

        string Name { get; set; }

        string Description { get; set; }

        DateTime LastModifyDate { get; }

        DateTime LastAccessDate { get; }

        bool UnsavedChanges { get; }

        IBaseData Copy();
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core.Observer
{
    [AttributeUsage(AttributeTargets.Class)]
    public  class ObservableClass : Attribute
    {
        private ObserverManager changeManager = new ObserverManager();

        [JsonIgnore]
        public ObserverManager ChangeManager => changeManager;

        [JsonIgnore]
        public bool UnsavedChanges
        {
            get => changeManager.UnsavedChanges;
        }

        public ObservableClass()
        {

        }
    }
}

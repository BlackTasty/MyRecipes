using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel.Observer;

namespace MyRecipes.Core
{
    // dsd
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseData<T> : JsonFile<T>, IBaseData
    {
        public event EventHandler<ChangeObservedEventArgs> ChangeObserved;

        protected string guid;
        protected string mName;
        protected string mDescription;
        private DateTime mLastModifyDate;
        private DateTime mLastAccessDate;

        public string Guid => guid;

        public string Name
        {
            get => mName;
            set
            {
                observerManager.ObserveProperty(value);
                mName = value;
                InvokePropertyChanged();
            }
        }

        public string Description
        {
            get => mDescription;
            set
            {
                observerManager.ObserveProperty(value);
                mDescription = value;
                InvokePropertyChanged();
            }
        }

        public DateTime LastModifyDate => mLastModifyDate;

        public DateTime LastAccessDate
        {
            get => mLastAccessDate;
            set
            {
                mLastAccessDate = value;
                InvokePropertyChanged();
            }
        }

        /// <summary>
        /// Load an existing object from JSON
        /// </summary>
        [JsonConstructor]
        public BaseData(string guid, string name, string description, DateTime lastModifyDate) : base(true)
        {
            this.guid = guid;
            Name = name;
            Description = description;
            mLastModifyDate = lastModifyDate;
            observerManager.GuidOverride = guid;
            observerManager.ChangeObserved += ChangeManager_ChangeObserved;
        }

        /// <summary>
        /// Generate a new object with GUID
        /// </summary>
        /// <param name="name">The name of this object</param>
        public BaseData(string name) : this(System.Guid.NewGuid().ToString(), name, "", DateTime.Now)
        {
            observerManager.IsEnabled = true;
        }

        public BaseData(FileInfo fi) : base(fi, true)
        {
            observerManager.GuidOverride = guid;
            observerManager.ChangeObserved += ChangeManager_ChangeObserved;
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual IBaseData Copy()
        {
            return new BaseData<T>(guid, Name, Description, LastModifyDate)
            {
                LastAccessDate = LastAccessDate
            };
        }

        protected override T LoadFile()
        {
            T loaded = base.LoadFile();
            
            if (loaded is IBaseData data)
            {
                Name = data.Name;
                guid = data.Guid;
                observerManager.GuidOverride = guid;
                Description = data.Description;
                mLastAccessDate = data.LastAccessDate;
                mLastModifyDate = data.LastModifyDate;
            }

            return loaded;
        }

        protected override void SaveFile(T @object)
        {
            mLastModifyDate = DateTime.Now;
            InvokePropertyChanged("LastModifyDate");
            base.SaveFile(@object);
        }

        protected override void SaveFile(string filePath, T @object)
        {
            mLastModifyDate = DateTime.Now;
            InvokePropertyChanged("LastModifyDate");
            base.SaveFile(filePath, @object);
        }

        private void ChangeManager_ChangeObserved(object sender, ChangeObservedEventArgs e)
        {
            OnChangeObserved(e);
        }

        protected void OnChangeObserved(ChangeObservedEventArgs e)
        {
            ChangeObserved?.Invoke(this, e);
        }

        IObservableClass IObservableClass.Copy()
        {
            return new BaseData<T>(guid, Name, Description, LastModifyDate)
            {
                LastAccessDate = LastAccessDate
            };
        }
    }
}

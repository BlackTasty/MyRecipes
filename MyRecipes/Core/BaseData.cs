using MyRecipes.Core.Observer;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
                changeManager.ObserveProperty(value);
                mName = value;
                InvokePropertyChanged();
            }
        }

        public string Description
        {
            get => mDescription;
            set
            {
                changeManager.ObserveProperty(value);
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
        public BaseData(string guid, string name, string description, DateTime lastModifyDate)
        {
            this.guid = guid;
            mName = name;
            mDescription = description;
            this.mLastModifyDate = lastModifyDate;
        }

        /// <summary>
        /// Generate a new object with GUID
        /// </summary>
        /// <param name="name">The name of this object</param>
        public BaseData(string name) : this(System.Guid.NewGuid().ToString(), name, "", DateTime.Now)
        {

        }

        public BaseData(FileInfo fi) : base(fi)
        {

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
    }
}

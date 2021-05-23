using MyRecipes.Core.Observer;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipes.Core
{
    //[ObservableClass]
    public class JsonFile<T> : ViewModelBase
    {
        protected string filePath;
        protected string fileName;
        protected bool fromFile;
        protected bool isFile;
        protected ObserverManager changeManager = new ObserverManager();

        [JsonIgnore]
        public ObserverManager ChangeManager => changeManager;

        [JsonIgnore]
        public bool UnsavedChanges
        {
            get => changeManager.UnsavedChanges;
        }

        /// <summary>
        /// Path excluding file name
        /// </summary>
        [JsonIgnore]
        public string FilePath => filePath;

        [JsonIgnore]
        public string FileName => fileName;

        [JsonIgnore]
        public bool FromFile => fromFile;

        public JsonFile() { }

        public JsonFile(FileInfo fi)
        {
            filePath = fi.DirectoryName;
            fileName = fi.Name;
            fi.Refresh();

            fromFile = true;
            isFile = true;
        }

        public JsonFile(DirectoryInfo di)
        {
            filePath = di.Parent.FullName;
            fileName = di.Name;
            di.Refresh();

            fromFile = true;
        }

        public virtual string GetFullPath()
        {
            return Path.Combine(filePath, fileName);
        }

        public virtual void Delete()
        {
            Delete(false);
        }

        public void Delete(bool forceDeleteParentDir = false)
        {
            // Return if object has not been saved to file yet
            if (!fromFile)
            {
                return;
            }

            if (isFile && !forceDeleteParentDir)
            {
                string path = Path.Combine(filePath, fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            else
            {
                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
            }
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        protected virtual void SaveFile(T @object)
        {
            string json = JsonConvert.SerializeObject(@object);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new Exception("FilePath must be set! Use SaveFile(string filePath, T @object) to create a new file instead!");
            }

            File.WriteAllText(Path.Combine(filePath, fileName), json);
            fromFile = true;
            FileAttributes attr = File.GetAttributes(Path.Combine(filePath, fileName));

            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
            {
                isFile = true;
            }
            changeManager.ResetObservers();
        }

        protected virtual void SaveFile(string filePath, T @object)
        {
            /*if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }*/

            this.filePath = filePath;
            SaveFile(@object);
        }

        protected virtual T LoadFile()
        {
            string targetPath = Path.Combine(filePath, fileName);
            if (!fromFile || !File.Exists(targetPath))
            {
                return default;
            }

            string json = File.ReadAllText(targetPath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

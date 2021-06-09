using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;

namespace MyRecipes.Core.Export
{
    class ExportSet<T> : ViewModelBase
    {
        public event EventHandler<EventArgs> ObjectIsSelectedChanged;

        private VeryObservableCollection<ExportObject<T>> mExportObjects = new VeryObservableCollection<ExportObject<T>>("ExportObjects");

        public VeryObservableCollection<ExportObject<T>> ExportObjects => mExportObjects;

        public int SelectedObjectsCount => mExportObjects.Where(x => x.IsSelected).Count();

        public bool? AllObjectsSelected
        {
            get
            {
                if (mExportObjects.All(x => x.IsSelected))
                {
                    return true;
                }
                else if (mExportObjects.All(x => !x.IsSelected))
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
        }

        public ExportSet(List<T> objectsToExport, bool isSelected)
        {
            if (objectsToExport == null)
            {
                return;
            }

            foreach (T objectToExport in objectsToExport)
            {
                if (objectToExport is BaseData<T> baseData)
                {
                    var exportObject = new ExportObject<T>(baseData, isSelected);
                    exportObject.IsSelectedChanged += ExportObject_IsSelectedChanged;
                    mExportObjects.Add(exportObject);
                }
            }
        }

        private void ExportObject_IsSelectedChanged(object sender, EventArgs e)
        {
            OnObjectIsSelectedChanged(e);
            InvokePropertiesChanged("ExportObjects");
            InvokePropertiesChanged("SelectedObjectsCount");
            InvokePropertiesChanged("AllObjectsSelected");
        }

        public ExportSet(T objectToExport) : this(new List<T>() { objectToExport }, true)
        {

        }

        public static ExportSet<T> Empty => new ExportSet<T>();

        private ExportSet() : this(null, false)
        {

        }

        public void SelectObjectForExport(T selected)
        {
            if (selected is JsonFile<T> jsonFile)
            {
                var target = mExportObjects.FirstOrDefault(x => x.Data.Equals(jsonFile));

                if (target != null)
                {
                    target.IsSelected = true;
                }
            }
        }

        public void SelectObjectsForExport(List<T> selected)
        {
            if (selected == null)
            {
                return;
            }

            foreach (T obj in selected)
            {
                SelectObjectForExport(obj);
            }
        }

        public void ExportSetTo(string folderPath, string fileName, string extension)
        {
            List<JsonFile<T>> selectedFiles = new List<JsonFile<T>>();

            foreach (ExportObject<T> selectedObject in mExportObjects.Where(x => x.IsSelected))
            {
                selectedFiles.Add(selectedObject.Data);
            }

            string json = JsonConvert.SerializeObject(selectedFiles);
            string filePath = Path.Combine(folderPath, string.Format("{0}.{1}", fileName, extension));

            File.WriteAllText(filePath, json);
        }

        protected virtual void OnObjectIsSelectedChanged(EventArgs e)
        {
            ObjectIsSelectedChanged?.Invoke(this, e);
        }
    }
}

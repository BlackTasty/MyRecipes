using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasty.ViewModel;

namespace MyRecipes.Core.Export
{
    class ExportObject<T> : ViewModelBase
    {
        public event EventHandler<EventArgs> IsSelectedChanged;

        private BaseData<T> data;
        private bool mIsSelected;

        public BaseData<T> Data => data;

        public bool IsSelected
        {
            get => mIsSelected;
            set
            {
                mIsSelected = value;
                InvokePropertiesChanged();
                OnIsSelectedChanged(EventArgs.Empty);
            }
        }

        public ExportObject(BaseData<T> data, bool isSelected)
        {
            this.data = data;
            mIsSelected = isSelected;
        }

        public void ExportTo(string folderPath, string fileName, string extension)
        {
            string filePath = Path.Combine(folderPath, string.Format("{0}.{1}", fileName, extension));

            File.WriteAllText(filePath, ToString());
        }

        public override string ToString()
        {
            return data.GetJson();
        }

        protected virtual void OnIsSelectedChanged(EventArgs e)
        {
            IsSelectedChanged?.Invoke(this, e);
        }
    }
}

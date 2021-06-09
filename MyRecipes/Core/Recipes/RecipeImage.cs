using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Tasty.ViewModel;
using Tasty.ViewModel.Observer;

namespace MyRecipes.Core.Recipes
{
    public class RecipeImage : ViewModelBase
    {
        private string filePath;
        private ImageSource image;
        protected ObserverManager observerManager = new ObserverManager();

        [JsonIgnore]
        public bool IsExporting { get; set; }

        [JsonIgnore]
        public ObserverManager ObserverManager => observerManager;

        [JsonIgnore]
        public bool UnsavedChanges
        {
            get => observerManager.UnsavedChanges;
        }

        [JsonIgnore]
        public bool IsImageSet => image != null;

        public string FilePath
        {
            get => filePath;
            set
            {
                if (!IsExporting)
                {
                    observerManager.ObserveProperty(value);
                }
                filePath = value;
                if (!IsExporting)
                {
                    image = Utils.FileToBitmapImage(value);
                    InvokePropertyChanged("Image");
                    InvokePropertyChanged("IsImageSet");
                }
            }
        }

        [JsonIgnore]
        public ImageSource Image
        {
            get => image;
        }

        [JsonConstructor]
        public RecipeImage(string filePath)
        {
            FilePath = filePath;
        }

        public RecipeImage(ImageSource image)
        {
            this.image = image;
            InvokePropertyChanged("Image");
        }

        public override string ToString()
        {
            return FilePath;
        }
    }
}

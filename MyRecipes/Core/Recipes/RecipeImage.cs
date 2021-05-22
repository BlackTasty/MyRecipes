using MyRecipes.Core.Observer;
using MyRecipes.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyRecipes.Core.Recipes
{
    public class RecipeImage : ViewModelBase
    {
        private string filePath;
        private ImageSource image;
        protected ObserverManager changeManager = new ObserverManager();

        [JsonIgnore]
        public ObserverManager ChangeManager => changeManager;

        [JsonIgnore]
        public bool UnsavedChanges
        {
            get => changeManager.UnsavedChanges;
        }

        public bool IsImageSet => image != null;

        public string FilePath
        {
            get => filePath;
            set
            {
                changeManager.ObserveProperty(value);
                filePath = value;
                image = Utils.FileToBitmapImage(value);
                InvokePropertyChanged("Image");
                InvokePropertyChanged("IsImageSet");
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
    }
}

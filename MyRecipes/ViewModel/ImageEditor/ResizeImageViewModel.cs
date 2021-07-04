using MyRecipes.Core;
using MyRecipes.Core.ImageEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tasty.ViewModel;
using Tasty.ViewModel.Observer;

namespace MyRecipes.ViewModel.ImageEditor
{
    class ResizeImageViewModel : ViewModelBase
    {
        protected ObserverManager observerManager = new ObserverManager();

        private BitmapImage mImage;

        private int mWidth;
        private int mHeight;

        public virtual BitmapImage Image
        {
            get => mImage;
            set
            {
                mImage = value;
                InvokePropertiesChanged();

                if (value != null)
                {
                    Width = (int)value.Width;
                    Height = (int)value.Height;
                    ResetHasChanges();
                }
            }
        }

        public bool HasChanges => observerManager.UnsavedChanges;

        public virtual int Width
        {
            get => mWidth;
            set
            {
                observerManager.ObserveProperty(value);
                mWidth = value;
                InvokePropertiesChanged();
                InvokePropertiesChanged("HasChanges");
            }
        }

        public virtual int WidthMax
        {
            get => (int)(mImage?.Width ?? 1);
        }

        public virtual int Height
        {
            get => mHeight;
            set
            {
                observerManager.ObserveProperty(value);
                mHeight = value;
                InvokePropertiesChanged();
                InvokePropertiesChanged("HasChanges");
            }
        }

        public virtual int HeightMax
        {
            get => (int)(mImage?.Height ?? 1);
        }

        public void ResetHasChanges()
        {
            observerManager.ResetObservers();
            InvokePropertiesChanged("HasChanges");
        }

        public virtual void ResetValues()
        {
            Width = observerManager.GetObserverByName("Width").GetOriginalValue();
            Height = observerManager.GetObserverByName("Height").GetOriginalValue();
            RefreshBindings();
        }

        public virtual BitmapImage ApplyTransform()
        {
            observerManager.ResetObservers();
            using (var resizer = new ImageResizing(mImage))
            {
                var resizedImage = resizer.Resize(mWidth, mHeight);
                var imageStream = resizedImage.ToStream();
                return Utils.ByteArrayToBitmapImage(imageStream.ToArray());
            }
        }

        public virtual void RefreshBindings()
        {
            InvokePropertiesChanged("Image");
            InvokePropertiesChanged("HasChanges");
            InvokePropertiesChanged("Width");
            InvokePropertiesChanged("WidthMax");
            InvokePropertiesChanged("Height");
            InvokePropertiesChanged("HeightMax");
        }
    }
}

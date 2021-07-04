using ControlzEx.Standard;
using MyRecipes.Core;
using MyRecipes.ViewModel.ImageEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tasty.ViewModel;

namespace MyRecipes.ViewModel
{
    class ImageEditorViewModel : ViewModelBase
    {
        private BitmapImage mResultImage;
        private Queue<byte[]> mUndoSteps = new Queue<byte[]>();
        private Queue<byte[]> mRedoSteps = new Queue<byte[]>();

        private bool mIsCropExpanded = true;
        private bool mIsResizeExpanded;
        private bool mIsFilterExpanded;

        private bool mShowImageCenter;
        private Rotation mImageRotation = Rotation.Rotate0;

        private CropImageViewModel cropVm = new CropImageViewModel();
        private ResizeImageViewModel resizeVm = new ResizeImageViewModel();

        public BitmapImage ResultImage
        {
            get => mResultImage;
            set
            {
                mResultImage = value;
                cropVm.Image = value;
                resizeVm.Image = value;
                InvokePropertiesChanged();
                InvokePropertiesChanged("CropVm");
                InvokePropertiesChanged("ResizeVm");
            }
        }

        public Rotation ImageRotation
        {
            get => mImageRotation;
            set
            {
                mImageRotation = value;

                double angle;
                switch (value)
                {
                    case Rotation.Rotate90:
                        angle = 90;
                        break;
                    case Rotation.Rotate180:
                        angle = 180;
                        break;
                    case Rotation.Rotate270:
                        angle = 270;
                        break;
                    default:
                        angle = 0;
                        break;
                }


                TransformedBitmap rotated = new TransformedBitmap(ResultImage, new RotateTransform(angle));

                string tempPath = Path.Combine(Path.GetTempPath(), "MyRecipes", "image_edit");
                ResultImage = Utils.SaveTransformedImage(rotated, tempPath, "rotated.tmp");
                InvokePropertiesChanged("ResultImage");
            }
        }

        public bool IsCropExpanded
        {
            get => mIsCropExpanded;
            set
            {
                mIsCropExpanded = value;
                InvokePropertiesChanged();

                if (value)
                {
                    IsResizeExpanded = false;
                    IsFilterExpanded = false;
                }
            }
        }

        public bool IsResizeExpanded
        {
            get => mIsResizeExpanded;
            set
            {
                mIsResizeExpanded = value;
                InvokePropertiesChanged();

                if (value)
                {
                    IsFilterExpanded = false;
                    IsCropExpanded = false;
                }
            }
        }

        public bool IsFilterExpanded
        {
            get => mIsFilterExpanded;
            set
            {
                mIsFilterExpanded = value;
                InvokePropertiesChanged();

                if (value)
                {
                    IsResizeExpanded = false;
                    IsCropExpanded = false;
                }
            }
        }

        public bool ShowImageCenter
        {
            get => mShowImageCenter;
            set
            {
                mShowImageCenter = value;
                InvokePropertiesChanged();
            }
        }

        public CropImageViewModel CropVm => cropVm;

        public ResizeImageViewModel ResizeVm => resizeVm;

        /*public void SetSubViewModels(CropImageViewModel cropVm, ResizeImageViewModel resizeVm)
        {
            this.cropVm = cropVm;
            this.cropVm.Image = mImage;

            this.resizeVm = resizeVm;
            this.resizeVm.Image = mImage;
        }*/

        public void RefreshBindings()
        {
            InvokePropertiesChanged("ResultImage");
            InvokePropertiesChanged("IsCropExpanded");
            InvokePropertiesChanged("IsResizeExpanded");
            InvokePropertiesChanged("IsFilterExpanded");
            InvokePropertiesChanged("ShowImageCenter");
            InvokePropertiesChanged("CropVm");
            InvokePropertiesChanged("ResizeVm");
            cropVm.RefreshBindings();
            resizeVm.RefreshBindings();
        }
    }
}

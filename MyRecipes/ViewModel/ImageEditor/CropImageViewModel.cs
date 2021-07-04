using MyRecipes.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tasty.ViewModel;
using Tasty.ViewModel.Observer;

namespace MyRecipes.ViewModel.ImageEditor
{
    class CropImageViewModel : ResizeImageViewModel
    {
        private BitmapSource mPreview;

        private int mX;
        private int mY;

        public override BitmapImage Image
        {
            get => base.Image;
            set
            {
                if (value != null)
                {
                    X = 0;
                    Y = 0;
                }
                base.Image = value;
            }
        }

        public virtual BitmapSource Preview
        {
            get => mPreview;
            set
            {
                mPreview = value;
                InvokePropertiesChanged();
            }
        }

        public override int Width
        {
            get => base.Width;
            set
            {
                base.Width = value;
                RefreshPreview();
                InvokePropertiesChanged("XMax");
            }
        }

        public override int WidthMax => Math.Max(base.WidthMax - X, 1);

        public override int Height
        {
            get => base.Height;
            set
            {
                base.Height = value;
                RefreshPreview();
                InvokePropertiesChanged("YMax");
            }
        }

        public override int HeightMax => Math.Max(base.HeightMax - Y, 1);

        public int X
        {
            get => mX;
            set
            {
                observerManager.ObserveProperty(value);
                mX = value;
                InvokePropertiesChanged();
                InvokePropertiesChanged("HasChanges");
                InvokePropertiesChanged("WidthMax");
                RefreshPreview();
            }
        }

        public int XMax
        {
            get => Width - 1;
        }

        public int Y
        {
            get => mY;
            set
            {
                observerManager.ObserveProperty(value);
                mY = value;
                InvokePropertiesChanged();
                InvokePropertiesChanged("HasChanges");
                InvokePropertiesChanged("HeightMax");
                RefreshPreview();
            }
        }

        public int YMax
        {
            get => Height - 1;
        }

        public override BitmapImage ApplyTransform()
        {
            CroppedBitmap cropped = new CroppedBitmap(Image, new Int32Rect(X, Y, Width, Height));

            string tempPath = Path.Combine(Path.GetTempPath(), "MyRecipes", "image_edit");
            return Utils.SaveTransformedImage(cropped, tempPath, "cropped.tmp");
        }

        public override void ResetValues()
        {
            base.ResetValues();

            X = observerManager.GetObserverByName("X").GetOriginalValue();
            Y = observerManager.GetObserverByName("Y").GetOriginalValue();
            RefreshBindings();
        }

        public override void RefreshBindings()
        {
            base.RefreshBindings();
            InvokePropertiesChanged("X");
            InvokePropertiesChanged("XMax");
            InvokePropertiesChanged("Y");
            InvokePropertiesChanged("YMax");
        }

        private void RefreshPreview()
        {
            if (Image == null)
            {
                return;
            }

            DrawingVisual visual = new DrawingVisual();
            Brush cutoutBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
            int realWidth = (int)Image.Width;
            int realHeight = (int)Image.Height;

            int diffLeft = X;
            int diffTop = Y;
            int diffRight = realWidth - Width - X;
            int diffBottom = realHeight - Height - Y;

            using (DrawingContext context = visual.RenderOpen())
            {
                context.DrawRectangle(null, new Pen(Brushes.Black, 2), new Rect(X, Y, Width, Height));

                // Draw top cutout rectangle
                if (diffTop > 0)
                {
                    context.DrawRectangle(cutoutBrush, null, 
                        new Rect(0, 
                                 0,
                                 realWidth,
                                 diffTop));
                }

                // Draw left cutout rectangle
                if (diffLeft > 0)
                {
                    context.DrawRectangle(cutoutBrush, null, 
                        new Rect(0, 
                                 diffTop, 
                                 diffLeft,
                                 realHeight - diffTop - diffBottom));
                }

                // Draw bottom cutout rectangle
                if (diffBottom > 0)
                {
                    context.DrawRectangle(cutoutBrush, null, 
                        new Rect(0,
                                 realHeight - diffBottom,
                                 realWidth, 
                                 diffBottom));
                }

                // Draw right cutout rectangle
                if (diffRight > 0)
                {
                    context.DrawRectangle(cutoutBrush, null, 
                        new Rect(realWidth - diffRight, 
                                 diffTop, 
                                 diffRight,
                                 realHeight - diffTop - diffBottom));
                }
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(realWidth, realHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(visual);
            rtb.Freeze();

            Preview = rtb;
            InvokePropertiesChanged("Preview");
        }
    }
}

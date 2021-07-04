using ControlzEx.Standard;
using MyRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyRecipes.Controls
{
    /// <summary>
    /// Interaction logic for ImageEditor.xaml
    /// </summary>
    public partial class ImageEditor : DockPanel
    {
        //public event 
        //FrameworkElement parentControl;

        public ImageEditor()
        {
            InitializeComponent();
            //this.parentControl = parentControl;
        }

        /*public ImageEditor(FrameworkElement parentControl, BitmapImage image)
        {
            InitializeComponent();
            (DataContext as ImageEditorViewModel).ResultImage = image;
            this.parentControl = parentControl;
        }*/

        public void EditImage(BitmapImage image)
        {
            ImageEditorViewModel vm = DataContext as ImageEditorViewModel;
            vm.ResultImage = image;
            vm.RefreshBindings();
            Visibility = Visibility.Visible;
        }

        private void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ApplyCrop_Click(object sender, RoutedEventArgs e)
        {
            ImageEditorViewModel vm = DataContext as ImageEditorViewModel;

            vm.ResultImage = vm.CropVm.ApplyTransform();
            vm.RefreshBindings();
        }

        private void ResetCrop_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ImageEditorViewModel).CropVm.ResetValues();
        }

        private void ApplyResize_Click(object sender, RoutedEventArgs e)
        {
            ImageEditorViewModel vm = DataContext as ImageEditorViewModel;

            vm.ResultImage = vm.ResizeVm.ApplyTransform();
            vm.RefreshBindings();
        }

        private void ResetResize_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ImageEditorViewModel).ResizeVm.ResetValues();
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {


            Close_Click(sender, e);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void image_crop_Expanded(object sender, RoutedEventArgs e)
        {
            image_filter.IsExpanded = false;
            image_resize.IsExpanded = false;
        }

        private void image_resize_Expanded(object sender, RoutedEventArgs e)
        {
            image_filter.IsExpanded = false;
            image_crop.IsExpanded = false;
        }

        private void image_filter_Expanded(object sender, RoutedEventArgs e)
        {
            image_crop.IsExpanded = false;
            image_resize.IsExpanded = false;
        }

        private void RotateImageLeft_Click(object sender, RoutedEventArgs e)
        {
            ImageEditorViewModel vm = DataContext as ImageEditorViewModel;
            switch (vm.ImageRotation)
            {
                case Rotation.Rotate0:
                    vm.ImageRotation = Rotation.Rotate270;
                    break;
                case Rotation.Rotate270:
                    vm.ImageRotation = Rotation.Rotate180;
                    break;
                case Rotation.Rotate180:
                    vm.ImageRotation = Rotation.Rotate90;
                    break;
                case Rotation.Rotate90:
                    vm.ImageRotation = Rotation.Rotate0;
                    break;
            }
        }

        private void RotateImageRight_Click(object sender, RoutedEventArgs e)
        {
            ImageEditorViewModel vm = DataContext as ImageEditorViewModel;
            switch (vm.ImageRotation)
            {
                case Rotation.Rotate0:
                    vm.ImageRotation = Rotation.Rotate90;
                    break;
                case Rotation.Rotate90:
                    vm.ImageRotation = Rotation.Rotate180;
                    break;
                case Rotation.Rotate180:
                    vm.ImageRotation = Rotation.Rotate270;
                    break;
                case Rotation.Rotate270:
                    vm.ImageRotation = Rotation.Rotate0;
                    break;
            }
        }

        private void ShowCenter_CheckedChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as ImageEditorViewModel).RefreshBindings();
        }
    }
}

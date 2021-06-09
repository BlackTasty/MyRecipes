using MahApps.Metro.Controls;
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
using Tasty.MaterialDesign.FilePicker;
using Tasty.MaterialDesign.FilePicker.Core;

namespace MyRecipes.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for ExportCookingDataDialog.xaml
    /// </summary>
    public partial class ExportCookingDataDialog : DockPanel
    {
        public ExportCookingDataDialog()
        {
            InitializeComponent();
        }

        private void OpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            FilePicker filePicker = new FilePicker()
            {
                Title = "Ziel-Ordner für Kochdaten-Datei angeben",
                //Filter = "Kochdaten-Datei|*.como",
                IsFolderSelect = true
            };
            filePicker.DialogClosed += FilePicker_DialogClosed;

            FilePicker.ShowDialog(filePicker, new MetroWindow());
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            ExportCookingDataViewModel vm = DataContext as ExportCookingDataViewModel;
            vm.RunExport();
        }

        private void FilePicker_DialogClosed(object sender, FilePickerClosedEventArgs e)
        {
            if (e.DialogResult == MessageBoxResult.OK)
            {
                (DataContext as ExportCookingDataViewModel).ExportPath = e.FilePath;
                txt_path.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            }
        }
    }
}

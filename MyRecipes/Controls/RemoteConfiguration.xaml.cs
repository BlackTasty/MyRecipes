using MyRecipes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for RemoteConfiguration.xaml
    /// </summary>
    public partial class RemoteConfiguration : DockPanel
    {
        public RemoteConfiguration()
        {
            InitializeComponent();
            RemoteConfigurationViewModel vm = DataContext as RemoteConfigurationViewModel;
            vm.LogChanged += LogChanged;
        }

        private void LogChanged(object sender, EventArgs e)
        {
            scroll_log.Dispatcher.Invoke(() =>
            {
                scroll_log.ScrollToBottom();
            });
        }

        private void StartStopServer_Click(object sender, RoutedEventArgs e)
        {
            RemoteConfigurationViewModel vm = DataContext as RemoteConfigurationViewModel;

            if (!vm.Server.IsRunning)
            {
                vm.Server.StartServer(vm.Username, txt_password.Password);
            }
            else
            {
                vm.Server.StopServer();
            }
        }

        private void RefreshIPAddress_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as RemoteConfigurationViewModel).RefreshIPAddress();
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as RemoteConfigurationViewModel).ClearLog();
        }
    }
}

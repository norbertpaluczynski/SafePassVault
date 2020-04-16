using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SafePassVault.App.Helpers;

namespace SafePassVault.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Helpers.Windows.LoginWindow = new LoginWindow();
            Helpers.Windows.LoginWindow.Show();
        }

        private void TopBar_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((Window)((DockPanel)sender).Tag).DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((Window)((Button)sender).Tag).Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            ((Window)((Button)sender).Tag).WindowState = WindowState.Minimized;
        }
       
    }
}

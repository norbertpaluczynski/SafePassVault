using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SafePassVault.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }
}

using SafePassVault.Core.Helpers;
using SafePassVault.Core.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Messages;

namespace SafePassVault.App.UserControls
{
    /// <summary>
    /// Interaction logic for EditServiceDialog.xaml
    /// </summary>
    public partial class EditServiceDialog : UserControl
    {
        public Service Service { get; set; }
        public Notifier Notifier { get; set; }

        public EditServiceDialog(Service service, Notifier notifier)
        {
            Service = service;
            Notifier = notifier;
            DataContext = Service;
            InitializeComponent();
            PasswordBox.Password = Service.Password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = PasswordGenerator.Generate(AppSettings.Settings.DefaultPasswordCharsetPreset, AppSettings.Settings.DefaultPasswordLength);
        }

        
    }
}

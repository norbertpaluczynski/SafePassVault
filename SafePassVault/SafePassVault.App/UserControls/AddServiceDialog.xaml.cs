using SafePassVault.Core.Models;
using SafePassVault.Core.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Messages;

namespace SafePassVault.App.UserControls
{
    /// <summary>
    /// Interaction logic for AddServiceDialog.xaml
    /// </summary>
    
    public partial class AddServiceDialog : UserControl
    {
        public Service Service { get; set; }
        public Notifier Notifier { get; set; }

        public AddServiceDialog(Notifier notifier)
        {
            Service = new Service();
            Notifier = notifier;
            DataContext = Service;
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Service.CreatedAt = DateTime.Now;
            Service.UpdatedAt = DateTime.Now;
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

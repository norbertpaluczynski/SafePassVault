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
        private Service _tempService;

        public Service Service { get; set; }
        public Notifier Notifier { get; set; }

        public EditServiceDialog(Service service, Notifier notifier)
        {
            Service = service;
            _tempService = new Service
            {
                Name = service.Name,
                Login = service.Login,
                Password = service.Password,
                Description = service.Description,
                Url = service.Url,
                CreatedAt = service.CreatedAt,
                UpdatedAt = service.UpdatedAt
            };

            Notifier = notifier;
            DataContext = _tempService;
            InitializeComponent();
            PasswordBox.Password = Service.Password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Service.Name = _tempService.Name;
            Service.Login = _tempService.Login;
            Service.Password = _tempService.Password;
            Service.Description = _tempService.Description;
            Service.Url = _tempService.Url;
            Service.CreatedAt = _tempService.CreatedAt;
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

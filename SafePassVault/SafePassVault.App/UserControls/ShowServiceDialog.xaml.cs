using SafePassVault.Core.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Messages;

namespace SafePassVault.App.UserControls
{
    /// <summary>
    /// Interaction logic for ShowServiceDialog.xaml
    /// </summary>
    public partial class ShowServiceDialog : UserControl
    {
        public Service Service { get; set; }
        public Notifier Notifier { get; set; }

        public ShowServiceDialog(Service service, Notifier notifier)
        {
            Service = service;
            Notifier = notifier;
            DataContext = Service;
            InitializeComponent();
        }

        private void OpenInBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(Service.Url))
            {
                try
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        UseShellExecute = true,
                        FileName = Service.Url
                    });
                }
                catch
                {
                    Notifier.ShowError("Website address is invalid!");
                }
            }
        }

        private void CopyLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Service.Login);
            Notifier.ShowSuccess("Login copied to clipboard!");
        }

        private void CopyPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Service.Password);
            Notifier.ShowSuccess("Password copied to clipboard!");
        }
    }
}

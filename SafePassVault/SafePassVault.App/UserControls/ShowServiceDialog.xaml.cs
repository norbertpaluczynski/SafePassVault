using SafePassVault.App.Helpers;
using SafePassVault.Core.Models;
using System.Diagnostics;
using System.Timers;
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
            PasswordExpirationDate.Text = Service.PasswordExpirationDate == null ? 
                "Not specified" : ((System.DateTime)Service.PasswordExpirationDate).ToString("dd-MM-yyyy");
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
            else
            {
                Notifier.ShowInformation("Website address has not been provided! ");
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
            MainWindow.ServiceListPage.CopyTimerStart();
        }
    }
}

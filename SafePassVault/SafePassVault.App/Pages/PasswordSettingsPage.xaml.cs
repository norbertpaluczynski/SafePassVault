using SafePassVault.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Messages;

namespace SafePassVault.App.Pages
{
    /// <summary>
    /// Interaction logic for PasswordSettingsPage.xaml
    /// </summary>
    public partial class PasswordSettingsPage : Page
    {
        public Notifier Notifier { get; set; }

        public PasswordSettingsPage(Notifier notifier)
        {
            Notifier = notifier;
            DataContext = AppSettings.Settings;
            InitializeComponent();
        }

        private void DefaultPasswordLengthSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            var password = DefaultPasswordLengthBox.Text;

            if (regex.IsMatch(password))
            {
                AppSettings.Settings.DefaultPasswordLength = Int32.Parse(password);
                Notifier.ShowSuccess("Saved changes!");
            }
            else
            {
                Notifier.ShowError("Invalid value!");
            }
        }
    }
}

using SafePassVault.Core.Models;
using SafePassVault.Core.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Messages;
using System.Text.RegularExpressions;

namespace SafePassVault.App.UserControls
{
    /// <summary>
    /// Interaction logic for AddServiceDialog.xaml
    /// </summary>
    
    public partial class AddServiceDialog : UserControl
    {
        public Service Service { get; private set; }
        public PasswordGeneratorSettings PasswordGeneratorSettings { get; private set; }
        public Notifier Notifier { get; set; }

        public AddServiceDialog(Notifier notifier)
        {
            Service = new Service();
            PasswordGeneratorSettings = new PasswordGeneratorSettings(AppSettings.Settings.DefaultPasswordGeneratorSettings);
            Notifier = notifier;
            DataContext = this;
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Service.CreatedAt = DateTime.Now;
            Service.UpdatedAt = DateTime.Now;
        }

        private void ShowPasswordSettings_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordGeneratorSettingsArea.Visibility == Visibility.Visible)
            {
                ShowPasswordSettingsButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.ChevronDoubleRight;
                ShowPasswordSettings.ToolTip = "Show password\ngenerator settings.";
                PasswordGeneratorSettingsArea.Visibility = Visibility.Collapsed;
            }
            else
            {
                ShowPasswordSettingsButtonIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.ChevronDoubleLeft;
                ShowPasswordSettings.ToolTip = "Hide password\ngenerator settings.";
                PasswordGeneratorSettingsArea.Visibility = Visibility.Visible;

            }
        }

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            var password = DefaultPasswordLengthBox.Text;

            if (regex.IsMatch(password))
            {
                if (password.Length < 4)
                {
                    PasswordGeneratorSettings.PasswordLength = Int32.Parse(password);

                    if(PasswordGeneratorSettings.PasswordLength > 4)
                    {
                        try
                        {
                            PasswordBox.Password = PasswordGenerator.Generate(PasswordGeneratorSettings);
                            Notifier.ShowSuccess("Password generated successfully!");
                        }
                        catch (Exception)
                        {
                            Notifier.ShowError("At least one checkbox must be checked!");
                        }
                    }
                    else
                    {
                        Notifier.ShowError("Password length must be longer than 4 characters!");
                    }
                }
                else
                {
                    Notifier.ShowError("Password length must be shorter than 1000 characters!");
                }
            }
            else
            {
                Notifier.ShowError("Password length must be a number!");
            }
        }
    }
}

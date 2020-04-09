using SafePassVault.Core.Helpers;
using SafePassVault.Core.Models;
using System;
using System.Text.RegularExpressions;
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
        private Service _service;

        public Service Service { get; private set; }
        public PasswordGeneratorSettings PasswordGeneratorSettings { get; private set; }        
        public Notifier Notifier { get; set; }

        public EditServiceDialog(Service service, Notifier notifier) 
        {
            _service = service;
            Service = new Service(service);
            PasswordGeneratorSettings = new PasswordGeneratorSettings(AppSettings.Settings.DefaultPasswordGeneratorSettings);
            Notifier = notifier;
            DataContext = this;
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _service.Name = Service.Name;
            _service.Login = Service.Login;
            _service.Password = Service.Password;
            _service.Description = Service.Description;
            _service.Url = Service.Url;
            _service.CreatedAt = Service.CreatedAt;
            _service.UpdatedAt = DateTime.Now;
        }

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            var password = DefaultPasswordLengthBox.Text;

            if (regex.IsMatch(password))
            {
                if(password.Length < 4)
                {
                    PasswordGeneratorSettings.PasswordLength = Int32.Parse(password);
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
                    Notifier.ShowError("Password length must be shorter than 1000 characters!");
                }
            }
            else
            {
                Notifier.ShowError("Password length must be a number!");
            }
        }

        private void ShowPasswordSettings_Click(object sender, RoutedEventArgs e)
        {
            if(PasswordGeneratorSettingsArea.Visibility == Visibility.Visible)
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
    }
}

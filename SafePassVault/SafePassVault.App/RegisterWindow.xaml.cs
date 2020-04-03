using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SafePassVault.Core.ApiClient;
using PCore.Cryptography;
using System.Diagnostics;
using SafePassVault.App.UserControls;
using MaterialDesignThemes.Wpf;

namespace SafePassVault.App
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private HttpClient _http;
        private Client _apiClient;

        public RegisterWindow()
        {
            _http = new HttpClient();
            _apiClient = new Client(_http);
            InitializeComponent();
        }

        private void ConfirmBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
            e.Command == ApplicationCommands.Cut ||
            e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(LoginBox.Text) || String.IsNullOrEmpty(PasswordBox.Password) || String.IsNullOrEmpty(ConfirmBox.Password))
            {
                var errorDialog = new MessageDialog("Fields cannot be empty!");
                await DialogHost.Show(errorDialog, "register");
                return;
            } else if (PasswordBox.Password != ConfirmBox.Password)
            {
                var errorDialog = new MessageDialog("Passwords are not the same!");
                await DialogHost.Show(errorDialog, "register");
                return;
            }
            PasswordHashingServiceProvider phsp = new PasswordHashingServiceProvider();

            UserRegisterPostModel registerModel = new UserRegisterPostModel
            {
                Username = LoginBox.Text,
                Password = await phsp.Client_ComputePasswordForLogin(Encoding.UTF8.GetBytes(LoginBox.Text), Encoding.UTF8.GetBytes(PasswordBox.Password)),
                PasswordRepeat = await phsp.Client_ComputePasswordForLogin(Encoding.UTF8.GetBytes(LoginBox.Text), Encoding.UTF8.GetBytes(ConfirmBox.Password))
            };

            try
            {
                var result = await _apiClient.ApiUsersRegisterAsync(registerModel);

                var successDialog = new MessageDialog("You registered successfuly!\nYou can log in now.");
                var dialogResult = await DialogHost.Show(successDialog, "register");

                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();
            }
            catch (ApiException ex) 
            {
                if(ex.StatusCode == 400)
                {
                    await DialogHost.Show(new MessageDialog("User with that login already exists!"), "register");
                } else
                {
                    await DialogHost.Show(new MessageDialog("Server error, exit code: " + ex.StatusCode), "register");
                }
            }
            catch (Exception)
            {
                await DialogHost.Show(new MessageDialog("Unknown error"), "register");
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            Close();
        }
    }
}

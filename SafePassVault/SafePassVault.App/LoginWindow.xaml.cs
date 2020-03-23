using MaterialDesignThemes.Wpf;
using PCore.Cryptography;
using SafePassVault.App.UserControls;
using SafePassVault.Core.ApiClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToastNotifications.Messages;

namespace SafePassVault.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private HttpClient _http;
        private Client _apiClient;
        public LoginWindow()
        {
            _http = new HttpClient();
            _apiClient = new Client(_http);
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordHashingServiceProvider phsp = new PasswordHashingServiceProvider();
            UserAuthenticatePostModel loginModel = new UserAuthenticatePostModel()
            {
                Username = LoginBox.Text,
                Password = await phsp.Client_ComputePasswordForLogin(Encoding.UTF8.GetBytes(LoginBox.Text), Encoding.UTF8.GetBytes(PasswordBox.Password))
            };

            try
            {
                var result = await _apiClient.ApiUsersAuthenticateAsync(loginModel);
                _ = result;

                MainWindow window = new MainWindow();
                window.Show();
                window.Notifier.ShowSuccess("You logged in successfully!");
                Close();
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
                FailLogin failDialog = new FailLogin();
                var dialogResult = await DialogHost.Show(failDialog, "login");
            }

            
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.Show();
            Close();
        }
    }
}

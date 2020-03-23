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
            UserRegisterPostModel registerModel = new UserRegisterPostModel
            { 
                Username = LoginBox.Text,
                Password = Encoding.UTF8.GetBytes(PasswordBox.Password),
                PasswordRepeat = Encoding.UTF8.GetBytes(ConfirmBox.Password)
            };

            try
            {
                var result = await _apiClient.ApiUsersRegisterAsync(registerModel);
            }
            catch { }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            Close();
        }
    }
}

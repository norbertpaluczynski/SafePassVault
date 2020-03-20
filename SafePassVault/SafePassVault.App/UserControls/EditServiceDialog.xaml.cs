using SafePassVault.App.Models;
using SafePassVault.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SafePassVault.App.UserControls
{
    /// <summary>
    /// Interaction logic for EditServiceDialog.xaml
    /// </summary>
    public partial class EditServiceDialog : UserControl
    {
        public Service Service { get; set; }

        public EditServiceDialog(Service service)
        {
            Service = service;
            DataContext = Service;
            InitializeComponent();
            PasswordBox.Password = Service.Password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Service.Name = ServiceBox.Text;
            Service.Login = LoginBox.Text;
            Service.Password = PasswordBox.Password;
            Service.Url = UrlBox.Text;
            Service.Description = DescriptionBox.Text;
            Service.UpdatedAt = DateTime.Now;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

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
    /// Interaction logic for AddServiceDialog.xaml
    /// </summary>
    
    public partial class AddServiceDialog : UserControl
    {
        public Service Service { get; set; }

        public AddServiceDialog()
        {
            Service = new Service();
            DataContext = Service;
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Service.Name);
            MessageBox.Show(Service.Login);
            MessageBox.Show(Service.Password);
            MessageBox.Show(Service.Description);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Service.Password = PasswordBox.Password;
        }

        private void PasswordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}

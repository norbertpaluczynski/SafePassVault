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
    /// Interaction logic for ErrorMessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : UserControl
    {

        public MessageDialog(string message)
        {
            InitializeComponent();
            errorText.Text = message;
        }
    }
}

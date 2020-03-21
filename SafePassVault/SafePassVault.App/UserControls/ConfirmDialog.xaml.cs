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
    /// Interaction logic for ConfirmDialog.xaml
    /// </summary>
    public partial class ConfirmDialog : UserControl
    {
        public bool IsConfirmed { get; set; }

        public ConfirmDialog()
        {
            IsConfirmed = false;
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
        }
    }
}

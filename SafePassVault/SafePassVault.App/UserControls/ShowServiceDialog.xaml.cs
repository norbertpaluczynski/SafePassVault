﻿using SafePassVault.App.Models;
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
    /// Interaction logic for ShowServiceDialog.xaml
    /// </summary>
    public partial class ShowServiceDialog : UserControl
    {
        public Service Service { get; set; }
        public ShowServiceDialog(Service serv)
        {
            Service = serv;
            DataContext = Service;
            InitializeComponent();
            PasswordBox.Password = Service.Password;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void PasswordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

using SafePassVault.App.Pages;
using SafePassVault.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using Newtonsoft.Json;
using System.IO;
using SafePassVault.App.Helpers;

namespace SafePassVault.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static StartPage _startPage;
        public static ServiceListPage _serviceListPage;
        public static PasswordSettingsPage _passwordSettingsPage;

        public Notifier Notifier { get; set; }

        public MainWindow()
        {
            Notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new ControlPositionProvider(
                    parentWindow: this,
                    trackingElement: MainGrid,
                    corner: Corner.BottomRight,
                    offsetX: 5,
                    offsetY: 5);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(1.5),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(3));

                cfg.DisplayOptions.Width = 230;

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            _startPage = new StartPage();
            _serviceListPage = new ServiceListPage(Notifier);
            _passwordSettingsPage = new PasswordSettingsPage(Notifier);

            InitializeComponent();
            ApplicationFrame.Content = _startPage;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow();
            window.Show();
            Close();            
        }

        private void ServiceListButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationFrame.Content = _serviceListPage;
        }

        private void PasswordSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationFrame.Content = _passwordSettingsPage;
        }
    }
}

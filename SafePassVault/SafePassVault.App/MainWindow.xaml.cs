using SafePassVault.App.Pages;
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
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace SafePassVault.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ServiceListPage _serviceListPage;
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
                    notificationLifetime: TimeSpan.FromSeconds(1),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(3));

                cfg.DisplayOptions.Width = 230;

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            _serviceListPage = new ServiceListPage(Notifier);

            InitializeComponent();
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

    }
}

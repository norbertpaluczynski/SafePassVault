using SafePassVault.App.Pages;
using System;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace SafePassVault.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StartPage _startPage;
        private readonly ServiceListPage _serviceListPage;
        private readonly PasswordSettingsPage _passwordSettingsPage;

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
            _startPage = new StartPage();
            _passwordSettingsPage = new PasswordSettingsPage();

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

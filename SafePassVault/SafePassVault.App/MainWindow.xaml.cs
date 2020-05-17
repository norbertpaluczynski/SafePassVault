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
using System.Timers;
using SafePassVault.Core.Helpers;
using ToastNotifications.Messages;
using MaterialDesignThemes.Wpf;
using SafePassVault.App.UserControls;
using SafePassVault.Core.ApiClient;

namespace SafePassVault.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer _timer;

        public static StartPage StartPage;
        public static ServiceListPage ServiceListPage;
        public static ChangePasswordPage ChangePasswordPage;

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
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.DisplayOptions.TopMost = false;

                cfg.DisplayOptions.Width = 230;

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            StartPage = new StartPage();
            ServiceListPage = new ServiceListPage(Notifier);
            ChangePasswordPage = new ChangePasswordPage(Notifier);

            InitializeComponent();
            ApplicationFrame.Content = StartPage;

            _timer = new Timer(1000);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timeLeft = AppSettings.Settings.IdleTimeLimit - TimeSpan.FromMilliseconds(IdleTimeFinder.GetIdleTime());

            if (timeLeft.Seconds > 0 && timeLeft.Seconds <= 3)
            {
                Notifier.ShowInformation($"You will be loged out in: { timeLeft.Seconds }!");
            }
            else if(timeLeft.Seconds <= 0)
            {                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Logout();
                });
            }
        }

        public void Logout()
        {
            _timer.Stop();
            WindowManager.LoginWindow = new LoginWindow();
            WindowManager.LoginWindow.Show();
            Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Logout();
        }

        private void ServiceListButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationFrame.Content = ServiceListPage;

            foreach (var service in ServiceListPage.Services)
            {
                if (service.PasswordExpirationDate < DateTime.Now)
                {
                    Notifier.ShowWarning($"Your {service.Name} password has expired!");
                }
            }
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationFrame.Content = ChangePasswordPage;
        }

        private async void DeleteUserAccountButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmWithPasswordDialog confirmDialog = new ConfirmWithPasswordDialog();
            await DialogHost.Show(confirmDialog, "root");

            if (confirmDialog.IsPasswordCorrect)
            {
                try
                {
                    await UserData.ApiClient.ApiUsersAsync();
                    Logout();
                    await DialogHost.Show(new MessageDialog("Account deleted succesfully!"), "login");
                }
                catch (ApiException<ProblemDetails> exc)
                {
                    foreach (var error in ApiErrorsBuilder.GetErrorList(exc.Result.Errors))
                    {
                        Notifier.ShowError(error);
                    }
                }
                catch (Exception)
                {
                    Notifier.ShowError("Unknown error");
                }
            }
            else
            {
                Notifier.ShowError("Password is not correct!");
            }
        }
    }
}

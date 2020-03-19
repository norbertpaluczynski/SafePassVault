using SafePassVault.App.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
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
using SafePassVault.App.UserControls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace SafePassVault.App.Pages
{
    /// <summary>
    /// Interaction logic for ServiceListPage.xaml
    /// </summary>
    public partial class ServiceListPage : Page
    {

        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(1),
                maximumNotificationCount: MaximumNotificationCount.FromCount(3));

            cfg.DisplayOptions.Width = 230;

            cfg.Dispatcher = Application.Current.Dispatcher;
        });


        public List<Service> services = new List<Service>();

        public ServiceListPage()
        {
            InitializeComponent();
            services.Add(new Service()
            {
                Name = "League of Legend",
                Login = "Gracz123",
                Password = "TOPsecretPASSWD",
                Url = "https://eune.leagueoflegends.com/pl-pl/",
                Description = "Konto z allegro"
            });
            services.Add(new Service()
            {
                Name = "League of Legend",
                Login = "SuperPlayer",
                Password = "123456789",
                Url = "https://eune.leagueoflegends.com/pl-pl/",
                Description = "Moje konto"
            });
            services.Add(new Service()
            {
                Name = "Steam",
                Login = "PanPawel",
                Password = "ToSieKameruje",
                Url = "https://store.steampowered.com/"
            }); services.Add(new Service()
            {
                Name = "League of Legend",
                Login = "Gracz123",
                Password = "TOPsecretPASSWD",
                Url = "https://eune.leagueoflegends.com/pl-pl/",
                Description = "Konto z allegro"
            });
            services.Add(new Service()
            {
                Name = "League of sadsadsadLegend",
                Login = "SuperPlayer",
                Password = "123456789",
                Url = "https://eune.leagueoflegends.com/pl-pl/",
                Description = "Moje konto"
            });
            services.Add(new Service()
            {
                Name = "Steam",
                Login = "PanPawel",
                Password = "ToSieKameruje",
                Url = "https://store.steampowered.com/"
            }); services.Add(new Service()
            {
                Name = "League of Legend",
                Login = "Gracz123",
                Password = "TOPsecretPASSWD",
                Url = "https://eune.leagueoflegends.com/pl-pl/",
                Description = "Konto z allegro"
            });
            services.Add(new Service()
            {
                Name = "Leaguesadasd of Legend",
                Login = "SuperPlayer",
                Password = "123456789",
                Url = "https://eune.leagueoflegends.com/pl-pl/",
                Description = "Moje konto"
            });
            services.Add(new Service()
            {
                Name = "Stsadsadsaeam",
                Login = "PanPawel",
                Password = "ToSieKameruje",
                Url = "https://store.steampowered.com/"
            }); services.Add(new Service()
            {
                Name = "League osadsadf Legend",
                Login = "Gracz123",
                Password = "TOPsecretPASSWD",
                Url = "https://eune.leagueoflegends.com/pl-pl/",
                Description = "Konto z allegro"
            });
            services.Add(new Service()
            {
                Name = "League of dsadaasdasdsaLegend",
                Login = "SuperPlayer",
                Password = "123456789",
                Url = "https://eune.leagueoflegends.com/pl-pl/",
                Description = "Moje konto"
            });
            services.Add(new Service()
            {
                Name = "Steam",
                Login = "PanPawel",
                Password = "ToSieKameruje",
                Url = "https://store.steampowered.com/"
            });
            RefreshServiceList();
        }

        public void RefreshServiceList()
        {
            ServiceList.ItemsSource = services;
        }

        private async void ShowServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
            ShowServiceDialog showService = new ShowServiceDialog(service);
            var result = await DialogHost.Show(showService, "root");
        }

        private async void EditServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
            EditServiceDialog editService = new EditServiceDialog(service);
            var result = await DialogHost.Show(editService, "root");
        }

        private void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
        }

        private async void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            AddServiceDialog addService = new AddServiceDialog();
            var result = await DialogHost.Show(addService, "root");
        }

        private void RefreshServiceButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshServiceList();
            notifier.ShowInformation("Refreshed!");
        }

        private void SyncServiceButton_Click(object sender, RoutedEventArgs e)
        {
            notifier.ShowInformation("Syncing...");
        }

        private void CopyLoginToClipboard(object sender, RoutedEventArgs e)
        {
            var service = (Service)((DataGridCell)e.Source).DataContext;
            Clipboard.SetText(service.Login);
            notifier.ShowSuccess("Login copied to clipboard!");
        }

        private void CopyPasswordToClipboard(object sender, RoutedEventArgs e)
        {
            var service = (Service)((DataGridCell)e.Source).DataContext;
            Clipboard.SetText(service.Password);
            notifier.ShowSuccess("Password copied to clipboard!");
        }
    }
}

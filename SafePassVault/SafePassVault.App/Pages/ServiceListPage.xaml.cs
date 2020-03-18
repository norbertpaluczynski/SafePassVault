using SafePassVault.Core.Models;
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

namespace SafePassVault.App.Pages
{
    /// <summary>
    /// Interaction logic for ServiceListPage.xaml
    /// </summary>
    public partial class ServiceListPage : Page
    {
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

        private void ShowServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
            MessageBox.Show(service.Name);
        }

        private void EditServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
        }

        private void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshServiceButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshServiceList();
        }

        private void SyncServiceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CopyLoginToClipboard(object sender, RoutedEventArgs e)
        {

        }

        private void CopyPasswordToClipboard(object sender, RoutedEventArgs e)
        {

        }
    }
}

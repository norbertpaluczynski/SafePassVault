using SafePassVault.App.Models;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SafePassVault.App.UserControls;
using ToastNotifications;
using ToastNotifications.Messages;
using System.Windows.Data;
using System.Collections.ObjectModel;
using SafePassVault.Core.Models;

namespace SafePassVault.App.Pages
{
    /// <summary>
    /// Interaction logic for ServiceListPage.xaml
    /// </summary>
    public partial class ServiceListPage : Page
    {
        public Notifier Notifier { get; set; }
        public ObservableCollection<Service> Services { get; set; }


        public ServiceListPage(Notifier notifier)
        {
            Services = new ObservableCollection<Service>();
            Notifier = notifier;
            InitializeComponent();
            DataContext = this;

            for(int i = 0; i < 10; i++)
            {
                Services.Add(new Service()
                {
                    Name = "League of Legend",
                    Login = "Gracz123",
                    Password = "TOPsecretPASSWD",
                    Url = "https://eune.leagueoflegends.com/pl-pl/",
                    Description = "Konto z allegro"
                });
            }
        }

        public void RefreshServiceList()
        {

        }

        private async void ShowServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
            ShowServiceDialog showService = new ShowServiceDialog(service, Notifier);
            var result = await DialogHost.Show(showService, "root");
        }

        private async void EditServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
            EditServiceDialog editService = new EditServiceDialog(service);
            var result = await DialogHost.Show(editService, "root");
        }

        private async void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDialog dialog = new ConfirmDialog();
            await DialogHost.Show(dialog, "root");

            if(dialog.IsConfirmed)
            {
                var service = (Service)((Button)e.Source).DataContext;
                Services.Remove(service);
            }
        }

        private async void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            AddServiceDialog addService = new AddServiceDialog();
            var result = await DialogHost.Show(addService, "root");
        }

        private void RefreshServiceButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshServiceList();
            Notifier.ShowInformation("Refreshed!");
        }

        private void SyncServiceButton_Click(object sender, RoutedEventArgs e)
        {
            Notifier.ShowInformation("Syncing...");
        }

        private void CopyLoginToClipboard(object sender, RoutedEventArgs e)
        {
            var service = (Service)((DataGridCell)e.Source).DataContext;
            Clipboard.SetText(service.Login);
            Notifier.ShowSuccess("Login copied to clipboard!");
        }

        private void CopyPasswordToClipboard(object sender, RoutedEventArgs e)
        {
            var service = (Service)((DataGridCell)e.Source).DataContext;
            Clipboard.SetText(service.Password);
            Notifier.ShowSuccess("Password copied to clipboard!");
        }
    }
}

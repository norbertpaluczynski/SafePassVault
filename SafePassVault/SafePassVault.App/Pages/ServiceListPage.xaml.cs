using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using SafePassVault.App.UserControls;
using ToastNotifications;
using ToastNotifications.Messages;
using System.Collections.ObjectModel;
using SafePassVault.Core.Models;
using PCore.Cryptography;
using System.Text;
using SafePassVault.App.Helpers;
using System.Security.Cryptography;
using SafePassVault.Core.ApiClient;
using System;
using Newtonsoft.Json;
using System.Diagnostics;

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
            Services = new ObservableCollection<Service>(UserData.GetServicesListDecrypted());
            Notifier = notifier;
            InitializeComponent();
            DataContext = this;
        }

        private async void ShowServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
            var showService = new ShowServiceDialog(service, Notifier);
            await DialogHost.Show(showService, "root");
        }

        private async void EditServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
            var editService = new EditServiceDialog(service, Notifier);
            var result = await DialogHost.Show(editService, "root");
            try
            {
                if ((bool)result)
                {
                    var eccService = new EccKeyServiceProvider();
                    var ServiceKeyPair = eccService.CreateNew_secp256r1_ECKeyPair();

                    var userKeyPair = UserData.eccKeyPairs[0];
                    var masterKeyService = new KeyDerivationServiceProvider();
                    var symEnc = new SymmetricCryptographyServiceProvider();

                    var derivedKey = eccService.EcdhDervieKey(
                        new EccKeyPairBlob(userKeyPair.PublicKey.Curve, userKeyPair.PublicKey.PublicKey, null),
                        ServiceKeyPair,
                        HashAlgorithmName.SHA256);

                    var masterKey = masterKeyService.Pbkdf2Sha256DeriveKeyFromPassword(derivedKey, 16, 16);

                    var encrypted = symEnc.Aes128GcmEncrypt(masterKey.MasterKey, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(editService.Service)));

                    var putModel = new EccCredentialPutModel()
                    {
                        EccDerivationBlob = new EccDerivationBlobModel()
                        {
                            Curve = ServiceKeyPair.Curve,
                            PublicKey = ServiceKeyPair.PublicKey
                        },
                        EccKeyPairId = userKeyPair.Id,
                        SymmetricCiphertextBlob = new SymmetricCiphertextBlobModel()
                        {
                            AuthenticationTag = encrypted.AuthenticationTag,
                            CipherDescription = encrypted.CipherDescription,
                            Ciphertext = encrypted.Cipthertext,
                            InitializationVector = encrypted.InitializationVector,
                            DerivationDescription = masterKey.DerivationDescription,
                            DerivationSalt = masterKey.DerivationSalt
                        },
                    };

                    await UserData.apiClient.ApiEcccredentialsPutAsync(service.Id, putModel);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Notifier.ShowError(ex.Message);
            }
        }

        private async void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var addService = new AddServiceDialog(Notifier);
            var result = await DialogHost.Show(addService, "root");

            try
            {
                if ((bool)result)
                {
                    var eccService = new EccKeyServiceProvider();
                    var serviceKeyPair = eccService.CreateNew_secp256r1_ECKeyPair();
                    
                    var userKeyPair = UserData.eccKeyPairs[0];
                    var masterKeyService = new KeyDerivationServiceProvider();
                    var symEnc = new SymmetricCryptographyServiceProvider();

                    var derivedKey = eccService.EcdhDervieKey(
                        new EccKeyPairBlob(userKeyPair.PublicKey.Curve, userKeyPair.PublicKey.PublicKey, null),
                        serviceKeyPair,
                        HashAlgorithmName.SHA256);

                    var masterKey = masterKeyService.Pbkdf2Sha256DeriveKeyFromPassword(derivedKey, 16, 16);

                    var encrypted = symEnc.Aes128GcmEncrypt(masterKey.MasterKey, Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(addService.Service)));

                    var postModel = new EccCredentialPostModel()
                    {
                        EccDerivationBlob = new EccDerivationBlobModel()
                        {
                            Curve = serviceKeyPair.Curve,
                            PublicKey = serviceKeyPair.PublicKey
                        },
                        EccKeyPairId = userKeyPair.Id,
                        SymmetricCiphertextBlob = new SymmetricCiphertextBlobModel()
                        {
                            AuthenticationTag = encrypted.AuthenticationTag,
                            CipherDescription = encrypted.CipherDescription,
                            Ciphertext = encrypted.Cipthertext,
                            InitializationVector = encrypted.InitializationVector,
                            DerivationDescription = masterKey.DerivationDescription,
                            DerivationSalt = masterKey.DerivationSalt
                        },
                    };
                    var PostResult = await UserData.apiClient.ApiEcccredentialsPostAsync(postModel);
                    Services.Add(addService.Service);
                }
            }
            catch(Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                Notifier.ShowError(ex.Message);
            }
        }

        private async void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDialog dialog = new ConfirmDialog();
            var result = await DialogHost.Show(dialog, "root");

            try
            {
                if ((bool)result)
                {
                    var service = (Service)((Button)e.Source).DataContext;
                    await UserData.apiClient.ApiEcccredentialsDeleteAsync(service.Id);
                    Services.Remove(service); 
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Notifier.ShowError(ex.Message);
            }
        }

        private void RefreshServiceButton_Click(object sender, RoutedEventArgs e)
        {
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

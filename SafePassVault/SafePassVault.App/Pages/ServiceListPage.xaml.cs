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

            EccKeyServiceProvider eccService = new EccKeyServiceProvider();
            var masterKeyService = new KeyDerivationServiceProvider();
            var symenc = new SymmetricCryptographyServiceProvider();
            var getUserServices = UserData.apiClient.ApiEcccredentialsGetAsync(null, null).ConfigureAwait(false).GetAwaiter().GetResult();
            var UserKeyPair = UserData.eccKeyPairs[0];

            foreach (var serviceTemp in getUserServices)
            {
                var derivedKey = eccService.EcdhDervieKey(
                    new EccKeyPairBlob(
                        serviceTemp.EccDerivationBlob.Curve,
                        serviceTemp.EccDerivationBlob.PublicKey,
                        null
                        ),
                    new EccKeyPairBlob(
                        serviceTemp.EccDerivationBlob.Curve,
                        null,
                        UserData.privateKeyDecrypted
                        ),
                    HashAlgorithmName.SHA256
                );
                var masterKey = masterKeyService.DeriveKeyFromBlob(
                    derivedKey,
                    new KeyDerivationBlob(
                        serviceTemp.SymmetricCiphertextBlob.DerivationDescription,
                        serviceTemp.SymmetricCiphertextBlob.DerivationSalt,
                        null
                        )
                    );

                var decryptedService = symenc.DecryptFromSymmetricCipthertextBlob(
                    masterKey.MasterKey,
                    new SymmetricCipthertextBlob(
                        serviceTemp.SymmetricCiphertextBlob.CipherDescription,
                        serviceTemp.SymmetricCiphertextBlob.InitializationVector,
                        serviceTemp.SymmetricCiphertextBlob.Ciphertext,
                        serviceTemp.SymmetricCiphertextBlob.AuthenticationTag
                        )
                );

                Services.Add(JsonConvert.DeserializeObject<Service>(Encoding.UTF8.GetString(decryptedService)));

            }

            

        }

        private async void ShowServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
            ShowServiceDialog showService = new ShowServiceDialog(service, Notifier);
            await DialogHost.Show(showService, "root");
        }

        private async void EditServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
            EditServiceDialog editService = new EditServiceDialog(service, Notifier);
            var result = await DialogHost.Show(editService, "root");

            try
            {
                if ((bool)result)
                {
                    //TODO
                }
            }
            catch { }
        }

        private async void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            AddServiceDialog addService = new AddServiceDialog(Notifier);
            var result = await DialogHost.Show(addService, "root");

            //try
            {
                if ((bool)result)
                {
                    //TODO
                    EccKeyServiceProvider eccService = new EccKeyServiceProvider();
                    var ServiceKeyPair = eccService.CreateNew_secp256r1_ECKeyPair();

                    // decrypt private keys
                    
                    var UserKeyPair = UserData.eccKeyPairs[0];
                    var masterKeyService = new KeyDerivationServiceProvider();
                    var symenc = new SymmetricCryptographyServiceProvider();

                    var derivedKey = eccService.EcdhDervieKey(
                        new EccKeyPairBlob(UserKeyPair.PublicKey.Curve, UserKeyPair.PublicKey.PublicKey, null),
                        ServiceKeyPair,
                        HashAlgorithmName.SHA256);

                    var masterKey = masterKeyService.Pbkdf2Sha256DeriveKeyFromPassword(derivedKey, 16, 16);

                    var encrypted = symenc.Aes128GcmEncrypt(masterKey.MasterKey, Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(addService.Service)));

                    var postModel = new EccCredentialPostModel()
                    {
                        EccDerivationBlob = new EccDerivationBlobModel()
                        {
                            Curve = ServiceKeyPair.Curve,
                            PublicKey = ServiceKeyPair.PublicKey
                        },
                        EccKeyPairId = UserKeyPair.Id,
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

                    string postModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(postModel);
                    var PostResult = await UserData.apiClient.ApiEcccredentialsPostAsync(postModel);

                    Services.Add(addService.Service);
                }
            }
            //catch { }
        }

        private async void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDialog dialog = new ConfirmDialog();
            var result = await DialogHost.Show(dialog, "root");

            try
            {
                if ((bool)result)
                {
                    //TODO
                    var service = (Service)((Button)e.Source).DataContext;
                    Services.Remove(service);
                }
            }
            catch { }
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

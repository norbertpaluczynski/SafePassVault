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
using SafePassVault.Core.Helpers;
using System.Timers;

namespace SafePassVault.App.Pages
{
    /// <summary>
    /// Interaction logic for ServiceListPage.xaml
    /// </summary>
    public partial class ServiceListPage : Page
    {
        private Timer CopyTimer;
        private Stopwatch StopWatch;

        public Notifier Notifier { get; set; }
        public ObservableCollection<Service> Services { get; set; }


        public ServiceListPage(Notifier notifier)
        {
            Services = new ObservableCollection<Service>(UserData.GetServicesListDecrypted());
            Notifier = notifier;
            InitializeComponent();
            DataContext = this;
            CopyTimer = new Timer(1000);
            StopWatch = new Stopwatch();
            CopyTimer.Elapsed += CopyTimer_Elapsed;
        }

        private async void ShowServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
            await DialogHost.Show(new ShowServiceDialog(service, Notifier), "root");
        }

        private async void EditServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)((Button)e.Source).DataContext;
            var dialog = new EditServiceDialog(service, Notifier);
            var result = await DialogHost.Show(dialog, "root");

            if (result == null)
            {
                return;
            }

            try
            {
                if ((bool)result)
                {
                    var eccService = new EccKeyServiceProvider();
                    var ServiceKeyPair = eccService.CreateNew_secp256r1_ECKeyPair();

                    var userKeyPair = UserData.eccKeyPairs[0];
                    var masterKeyService = new KeyDerivationServiceProvider();
                    var crypto = new SymmetricCryptographyServiceProvider();

                    var derivedKey = eccService.EcdhDervieKey(
                        new EccKeyPairBlob(userKeyPair.PublicKey.Curve, userKeyPair.PublicKey.PublicKey, null),
                        ServiceKeyPair,
                        HashAlgorithmName.SHA256);

                    var masterKey = masterKeyService.Pbkdf2Sha256DeriveKeyFromPassword(derivedKey, 16, 16);

                    var encrypted = crypto.Aes128GcmEncrypt(masterKey.MasterKey, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(service)));

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

                    await UserData.ApiClient.ApiEcccredentialsPutAsync(service.Id, putModel);
                }
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

        private async void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddServiceDialog(Notifier);
            var result = await DialogHost.Show(dialog, "root");

            if (result == null)
            {
                return;
            }

            try
            {
                if ((bool)result)
                {
                    var eccService = new EccKeyServiceProvider();
                    var serviceKeyPair = eccService.CreateNew_secp256r1_ECKeyPair();
                    
                    var userKeyPair = UserData.eccKeyPairs[0];
                    var masterKeyService = new KeyDerivationServiceProvider();
                    var crypto = new SymmetricCryptographyServiceProvider();

                    var derivedKey = eccService.EcdhDervieKey(
                        new EccKeyPairBlob(userKeyPair.PublicKey.Curve, userKeyPair.PublicKey.PublicKey, null),
                        serviceKeyPair,
                        HashAlgorithmName.SHA256);

                    var masterKey = masterKeyService.Pbkdf2Sha256DeriveKeyFromPassword(derivedKey, 16, 16);

                    var encrypted = crypto.Aes128GcmEncrypt(masterKey.MasterKey, Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(dialog.Service)));

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
                    var PostResult = await UserData.ApiClient.ApiEcccredentialsPostAsync(postModel);
                    Services.Add(dialog.Service);
                }
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

        private async void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await DialogHost.Show(new ConfirmDialog(), "root");
            
            if (result == null)
            {
                return;
            }

            try
            {
                if ((bool)result)
                {
                    var service = (Service)((Button)e.Source).DataContext;
                    await UserData.ApiClient.ApiEcccredentialsDeleteAsync(service.Id);
                    Services.Remove(service); 
                }
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
            CopyTimerStart();
        }

        public void CopyTimerStart()
        {
            Notifier.ShowSuccess("Password copied to clipboard for 15 sec!");
            StopWatch.Reset();
            CopyTimer.Stop();

            StopWatch.Start();
            CopyTimer.Start();
            CopyProgressBar.Visibility = Visibility.Visible;
            CopyProgressBar.Value = 15;
        }


        private void CopyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CopyProgressBar.Value -= 1;
            });
            
            if (StopWatch.Elapsed.Seconds > 14)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Clipboard.Clear();
                    CopyProgressBar.Visibility = Visibility.Hidden;
                });
                StopWatch.Stop();
                StopWatch.Reset();
                CopyTimer.Stop();
                Notifier.ShowInformation("Clipboard cleaned!");
            }
        }
    }
}

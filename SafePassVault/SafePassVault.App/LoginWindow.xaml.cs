using MaterialDesignThemes.Wpf;
using PCore.Cryptography;
using SafePassVault.App.UserControls;
using SafePassVault.Core.ApiClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToastNotifications.Messages;
using SafePassVault.App.Helpers;
using System.Security.Cryptography;

namespace SafePassVault.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(LoginBox.Text) || String.IsNullOrEmpty(PasswordBox.Password))
            {
                await DialogHost.Show(new MessageDialog("Fields cannot be empty!"), "login");
                return;
            }

            PasswordHashingServiceProvider phsp = new PasswordHashingServiceProvider();
            UserAuthenticatePostModel loginModel = new UserAuthenticatePostModel()
            {
                Username = LoginBox.Text,
                Password = await phsp.Client_ComputePasswordForLogin(Encoding.UTF8.GetBytes(LoginBox.Text), Encoding.UTF8.GetBytes(PasswordBox.Password))
            };

            try
            {                
                var result = await UserData.ApiClient.ApiUsersAuthenticateAsync(loginModel);

                UserData.AuthToken = result.AuthenticationToken;
                UserData.BytePassword = Encoding.UTF8.GetBytes(PasswordBox.Password);
                
                var checkResult = await UserData.ApiClient.ApiEcckeypairsGetAsync(20, 0);

                if(checkResult.Count == 0) 
                {
                    // Send new keypair
                    EccKeyServiceProvider ecckey = new EccKeyServiceProvider();
                    var keypair = ecckey.CreateNew_secp256r1_ECKeyPair();

                    var masterKeyService = new KeyDerivationServiceProvider();
                    var masterKey = masterKeyService.Pbkdf2Sha256DeriveKeyFromPassword(UserData.BytePassword, 16, 16);

                    var crypto = new SymmetricCryptographyServiceProvider();
                    var privateKeyEncrypted = crypto.Aes128GcmEncrypt(masterKey.MasterKey, keypair.PrivateKey);

                    var keyPairPostResult = await UserData.ApiClient.ApiEcckeypairsPostAsync(new EccKeyPairPostModel
                    {
                        EncryptedPrivateKey = new EccEncryptedPrivateKeyModel()
                        {
                            Curve = keypair.Curve,
                            AuthenticationTag = privateKeyEncrypted.AuthenticationTag,
                            CipherDescription = privateKeyEncrypted.CipherDescription,
                            Ciphertext = privateKeyEncrypted.Cipthertext,
                            InitializationVector = privateKeyEncrypted.InitializationVector,
                            DerivationDescription = masterKey.DerivationDescription,
                            DerivationSalt = masterKey.DerivationSalt
                        },
                        PublicKey = new EccPublicKeyModel()
                        {
                            Curve = keypair.Curve,
                            PublicKey = keypair.PublicKey
                        }
                    });
                }
                else
                {
                    // Get user keypairs and decrypt them
                    foreach(var keyPair in checkResult)
                    {
                        UserData.eccKeyPairs.Add(keyPair);
                        var masterKeyService = new KeyDerivationServiceProvider();
                        var masterKey = masterKeyService.DeriveKeyFromBlob(UserData.BytePassword, new KeyDerivationBlob(
                            keyPair.EncryptedPrivateKey.DerivationDescription,
                            keyPair.EncryptedPrivateKey.DerivationSalt,
                            null
                            ));

                        var crypto = new SymmetricCryptographyServiceProvider();

                        var privateKeyDecrypted = crypto.DecryptFromSymmetricCipthertextBlob(masterKey.MasterKey, new SymmetricCipthertextBlob
                            (
                                keyPair.EncryptedPrivateKey.CipherDescription,
                                keyPair.EncryptedPrivateKey.InitializationVector,
                                keyPair.EncryptedPrivateKey.Ciphertext,
                                keyPair.EncryptedPrivateKey.AuthenticationTag
                            )
                        );

                        UserData.PrivateKeyDecrypted = privateKeyDecrypted;
                        //CngKey.Import(privateKeyDecrypted, CngKeyBlobFormat.EccPrivateBlob, new CngProvider());
                    }
                }

                Windows.MainWindow = new MainWindow();
                Windows.MainWindow.Show();
                Windows.MainWindow.Notifier.ShowSuccess("You logged in successfully!");
                Close();
            }
            catch (ApiException)
            {
                await DialogHost.Show(new MessageDialog("Wrong username or password!\nPlease try again."), "login");
            }
            catch (Exception)
            {
                await DialogHost.Show(new MessageDialog("Unknown error"), "login");
            }

            
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Windows.RegisterWindow = new RegisterWindow();
            Windows.RegisterWindow.Show();
            Close();
        }
    }
}

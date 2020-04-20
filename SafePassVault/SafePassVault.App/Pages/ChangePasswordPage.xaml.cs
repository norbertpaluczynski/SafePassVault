using MaterialDesignThemes.Wpf;
using PCore.Cryptography;
using SafePassVault.App.Helpers;
using SafePassVault.App.UserControls;
using SafePassVault.Core.ApiClient;
using SafePassVault.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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
using ToastNotifications;
using ToastNotifications.Messages;

namespace SafePassVault.App.Pages
{
    /// <summary>
    /// Interaction logic for ChangePasswordPage.xaml
    /// </summary>
    public partial class ChangePasswordPage : Page
    {
        public Notifier Notifier { get; set; }

        public ChangePasswordPage(Notifier notifier)
        {
            Notifier = notifier;
            InitializeComponent();
        }

        private async void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(OldPasswordBox.Password) ||
                String.IsNullOrEmpty(NewPasswordBox.Password) ||
                String.IsNullOrEmpty(ConfirmNewPasswordBox.Password))
            {
                Notifier.ShowError("Fields can not be empty!");
                return;
            }

            var keyPairs = new List<EccKeyPairModel>();

            foreach(var keyPair in UserData.eccKeyPairs)
            {
                var masterKeyService = new KeyDerivationServiceProvider();
                var decryptionMasterKey = masterKeyService.DeriveKeyFromBlob(UserData.BytePassword, new KeyDerivationBlob(
                    keyPair.EncryptedPrivateKey.DerivationDescription,
                    keyPair.EncryptedPrivateKey.DerivationSalt,
                    null
                    ));

                var crypto = new SymmetricCryptographyServiceProvider();

                var privateKeyDecrypted = crypto.DecryptFromSymmetricCipthertextBlob(decryptionMasterKey.MasterKey, new SymmetricCipthertextBlob
                    (
                        keyPair.EncryptedPrivateKey.CipherDescription,
                        keyPair.EncryptedPrivateKey.InitializationVector,
                        keyPair.EncryptedPrivateKey.Ciphertext,
                        keyPair.EncryptedPrivateKey.AuthenticationTag
                    )
                );

                var encryptionMasterKey = masterKeyService.Pbkdf2Sha256DeriveKeyFromPassword(Encoding.UTF8.GetBytes(NewPasswordBox.Password), 16, 16);
                var encryptedPrivateKey = crypto.Aes128GcmEncrypt(encryptionMasterKey.MasterKey, privateKeyDecrypted);

                keyPairs.Add(new EccKeyPairModel()
                {
                    Id = keyPair.Id,
                    PublicKey = keyPair.PublicKey,
                    EncryptedPrivateKey = new EccEncryptedPrivateKeyModel()
                    {
                        Curve = keyPair.EncryptedPrivateKey.Curve,
                        AuthenticationTag = encryptedPrivateKey.AuthenticationTag,
                        CipherDescription = encryptedPrivateKey.CipherDescription,
                        Ciphertext = encryptedPrivateKey.Cipthertext,
                        InitializationVector = encryptedPrivateKey.InitializationVector,
                        DerivationDescription = encryptionMasterKey.DerivationDescription,
                        DerivationSalt = encryptionMasterKey.DerivationSalt
                    }
                });
            }

            try
            {
                PasswordHashingServiceProvider phsp = new PasswordHashingServiceProvider();

                await UserData.ApiClient.ApiUsersChangePasswordAsync(new Core.ApiClient.UserChangePasswordModel()
                {                    
                    CurrentPassword = await phsp.Client_ComputePasswordForLogin(Encoding.UTF8.GetBytes(UserData.UserName), Encoding.UTF8.GetBytes(OldPasswordBox.Password)),
                    Password = await phsp.Client_ComputePasswordForLogin(Encoding.UTF8.GetBytes(UserData.UserName), Encoding.UTF8.GetBytes(NewPasswordBox.Password)),
                    PasswordRepeat = await phsp.Client_ComputePasswordForLogin(Encoding.UTF8.GetBytes(UserData.UserName), Encoding.UTF8.GetBytes(ConfirmNewPasswordBox.Password)),
                    EccKeyPairs = keyPairs
                });                              

                WindowManager.LoginWindow = new LoginWindow();
                WindowManager.LoginWindow.Show();
                WindowManager.MainWindow.Close();
                var dialogResult = await DialogHost.Show(new MessageDialog("Password changed successfully!\nYou can login now."), "login");
            }
            catch(ApiException<ProblemDetails> exc)
            {
                foreach(var error in ApiErrorsBuilder.GetErrorList(exc.Result.Errors))
                {
                    Notifier.ShowError(error);
                }
            }
            catch (Exception)
            {
                Notifier.ShowError("Unknown error");
            }
        }
    }
}

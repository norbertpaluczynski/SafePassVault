using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using PCore.Cryptography;
using SafePassVault.Core.ApiClient;
using SafePassVault.Core.Models;

namespace SafePassVault.App.Helpers
{
    public static class UserData
    {
        public static HttpClient Http;
        public static Client ApiClient;
        static public byte[] BytePassword;
        static public byte[] PrivateKeyDecrypted;

        static public string AuthToken 
        {
            set 
            {
                Http.DefaultRequestHeaders.Remove("Authorization");
                Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {value}");
            }
        }

        static public List<EccKeyPairGetModel> eccKeyPairs;

        static UserData()
        {
            Http = new HttpClient();
            ApiClient = new Client(Http);
            eccKeyPairs = new List<EccKeyPairGetModel>();
        }

        public static List<Service> GetServicesListDecrypted()
        {
            var services = new List<Service>();
            var eccService = new EccKeyServiceProvider();
            var masterKeyService = new KeyDerivationServiceProvider();
            var crypto = new SymmetricCryptographyServiceProvider();
            var userServices = UserData.ApiClient.ApiEcccredentialsGetAsync(null, null).ConfigureAwait(false).GetAwaiter().GetResult();

            foreach (var service in userServices)
            {
                var derivedKey = eccService.EcdhDervieKey(
                    new EccKeyPairBlob(
                        service.EccDerivationBlob.Curve,
                        service.EccDerivationBlob.PublicKey,
                        null
                        ),
                    new EccKeyPairBlob(
                        service.EccDerivationBlob.Curve,
                        null,
                        UserData.PrivateKeyDecrypted
                        ),
                    HashAlgorithmName.SHA256
                );

                var masterKey = masterKeyService.DeriveKeyFromBlob(
                    derivedKey,
                    new KeyDerivationBlob(
                        service.SymmetricCiphertextBlob.DerivationDescription,
                        service.SymmetricCiphertextBlob.DerivationSalt,
                        null
                        )
                    );

                var decryptedService = crypto.DecryptFromSymmetricCipthertextBlob(
                    masterKey.MasterKey,
                    new SymmetricCipthertextBlob(
                        service.SymmetricCiphertextBlob.CipherDescription,
                        service.SymmetricCiphertextBlob.InitializationVector,
                        service.SymmetricCiphertextBlob.Ciphertext,
                        service.SymmetricCiphertextBlob.AuthenticationTag
                        )
                );

                Service tempService = JsonConvert.DeserializeObject<Service>(Encoding.UTF8.GetString(decryptedService));
                tempService.Id = service.Id;
                services.Add(tempService);
            }

            return services;
        }   
    }
}

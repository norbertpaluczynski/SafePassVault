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
        public static HttpClient http;
        public static Client apiClient;
        static public byte[] bytePassword;
        static public byte[] privateKeyDecrypted;
        static public string AuthToken 
        {
            set 
            {
                http.DefaultRequestHeaders.Remove("Authorization");
                http.DefaultRequestHeaders.Add("Authorization", $"Bearer {value}");
            }
        }
        static public List<EccKeyPairGetModel> eccKeyPairs;

        static UserData()
        {
            http = new HttpClient();
            apiClient = new Client(http);
            eccKeyPairs = new List<EccKeyPairGetModel>();
        }

        public static List<Service> GetServicesListDecrypted()
        {
            var services = new List<Service>();
            var eccService = new EccKeyServiceProvider();
            var masterKeyService = new KeyDerivationServiceProvider();
            var symenc = new SymmetricCryptographyServiceProvider();
            var getUserServices = UserData.apiClient.ApiEcccredentialsGetAsync(null, null).ConfigureAwait(false).GetAwaiter().GetResult();
            //var userKeyPair = UserData.eccKeyPairs[0];

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
                Service tempServ = JsonConvert.DeserializeObject<Service>(Encoding.UTF8.GetString(decryptedService));
                tempServ.Id = serviceTemp.Id;
                services.Add(tempServ);
            }
            return services;
        }   
    }
}

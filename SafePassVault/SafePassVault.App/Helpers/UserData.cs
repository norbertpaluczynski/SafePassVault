using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using PCore.Cryptography;
using SafePassVault.Core.ApiClient;

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
    }
}

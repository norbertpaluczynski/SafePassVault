using Newtonsoft.Json;
using SafePassVault.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SafePassVault.App.Helpers
{
    public class JsonData 
    {
        public List<Service> list { get; set; }

    }
    public static class StaticHelper
    {
        public static void SaveData()
        {
            try
            {
                JsonData data = new JsonData();
                data.list = new List<Service>();
                String username = LoginWindow._username;

                foreach (var x in MainWindow._serviceListPage.Services)
                {
                    data.list.Add(x);
                }


                File.WriteAllText("C:\\Users\\Norbert\\AppData\\Local\\Temp\\" + username + ".json", JsonConvert.SerializeObject(data));
            }
            catch
            {

            }
        }

        public static List<Service> ReadData()
        {
            try
            {
                String username = LoginWindow._username;
                String path = "C:\\Users\\Norbert\\AppData\\Local\\Temp\\" + username + ".json";
                if (File.Exists(path))
                {
                    String read = File.ReadAllText("C:\\Users\\Norbert\\AppData\\Local\\Temp\\" + username + ".json");

                    var list = JsonConvert.DeserializeObject<JsonData>(read);
                    return list.list;
                }
                else
                {
                    return null;
                }
            }
            catch
            {

            }
            return null;
        }
    }
}

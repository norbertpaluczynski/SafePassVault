using System;
using System.Collections.Generic;
using System.Text;

namespace SafePassVault.Core.Helpers
{
    public static class ApiErrorsBuilder
    {
        public static List<string> GetErrorList(IDictionary<string, string[]> dictionary)
        {
            var errorsList = new List<string>();

            foreach(var key in dictionary.Keys)
            {
                foreach(var error in dictionary[key])
                {
                    if(!errorsList.Contains(error))
                    {
                        errorsList.Add(error);
                    }
                }
            }

            return errorsList;
        }

        public static string GetErrorString(IDictionary<string, string[]> dictionary)
        {
            var list = GetErrorList(dictionary);

            StringBuilder stringBuilder = new StringBuilder();

            foreach(var error in list)
            {
                stringBuilder.Append(error);
            }

            return stringBuilder.ToString();
        }
    }
}

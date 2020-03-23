using SafePassVault.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace SafePassVault.Core.Helpers
{
    public static class PasswordGenerator
    {
        public static string Generate(PasswordCharsetPreset preset, int length)
        {
            var allowedCharset = GetAllowedCharsets(preset);
            var array = ShuffleArray(allowedCharset);

            StringBuilder password = new StringBuilder();

            for(int i = 0; i < length; i++)
            {
                password.Append(array[RandomNumberGenerator.GetInt32(int.MaxValue) % array.Length]);
            }

            return password.ToString();
        }

        private static char[] GetAllowedCharsets(PasswordCharsetPreset preset)
        {
            StringBuilder builder = new StringBuilder();

            if(preset.AllowNumbers)
            {
                builder.Append(PasswordCharsets.Numbers);
            }
            if (preset.AllowLowercaseLetters)
            {
                builder.Append(PasswordCharsets.LowercaseLetters);
            }
            if (preset.AllowUppercaseLetters)
            {
                builder.Append(PasswordCharsets.UppercaseLetters);
            }
            if (preset.AllowSpecialCharacters)
            {
                builder.Append(PasswordCharsets.SpecialCharacters);
            }
            if (preset.AllowSpace)
            {
                builder.Append(PasswordCharsets.Space);
            }

            return builder.ToString().ToCharArray();
        }

        private static char[] ShuffleArray(char[] array)
        {
            char temp;
            int index;
            
            for (int i = 0; i < array.Length; i++)
            {
                index = RandomNumberGenerator.GetInt32(array.Length);
                temp = array[i];
                array[i] = array[index];
                array[index] = temp;
            }            
            
            return array;
        }
    }
}

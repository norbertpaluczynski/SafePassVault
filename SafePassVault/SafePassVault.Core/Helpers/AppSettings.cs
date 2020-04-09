using SafePassVault.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafePassVault.Core.Helpers
{
    public class AppSettings
    {
        public static AppSettings Settings = new AppSettings();

        public PasswordGeneratorSettings DefaultPasswordGeneratorSettings { get; private set; } = new PasswordGeneratorSettings
        {
            AllowNumbers = true,
            AllowLowercaseLetters = true,
            AllowUppercaseLetters = true,
            AllowSpecialCharacters = true,
            AllowSpace = true,
            PasswordLength = 12
        };

    }
}

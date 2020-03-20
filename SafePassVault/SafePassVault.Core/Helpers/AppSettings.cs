using SafePassVault.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafePassVault.Core.Helpers
{
    public class AppSettings
    {
        public static AppSettings Settings = new AppSettings();

        public PasswordCharsetPreset DefaultPasswordCharsetPreset { get; private set; } = new PasswordCharsetPreset
        {
            PresetName = "Default",
            AllowNumbers = true,
            AllowLowercaseLetters = true,
            AllowUppercaseLetters = true,
            AllowSpecialCharacters = true,
            AllowSpace = true
        };
    }
}

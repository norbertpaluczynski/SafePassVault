using SafePassVault.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SafePassVault.Core.Helpers
{
    public static class AppSettings
    {
        public static PasswordCharsetPreset DefaultPasswordCharsetPreset => new PasswordCharsetPreset
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

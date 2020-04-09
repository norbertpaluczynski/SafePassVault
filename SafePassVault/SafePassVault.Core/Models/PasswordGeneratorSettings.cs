using System;
using System.Collections.Generic;
using System.Text;

namespace SafePassVault.Core.Models
{
    public class PasswordGeneratorSettings
    {
        public bool AllowNumbers { get; set; }
        public bool AllowLowercaseLetters { get; set; }
        public bool AllowUppercaseLetters { get; set; }
        public bool AllowSpecialCharacters { get; set; }
        public bool AllowSpace { get; set; }
        public int PasswordLength { get; set; }

        public PasswordGeneratorSettings()
        {
            PasswordLength = 12;
        }

        public PasswordGeneratorSettings(PasswordGeneratorSettings settings)
        {
            AllowNumbers = settings.AllowNumbers;
            AllowLowercaseLetters = settings.AllowLowercaseLetters;
            AllowUppercaseLetters = settings.AllowUppercaseLetters;
            AllowSpecialCharacters = settings.AllowSpecialCharacters;
            AllowSpace = settings.AllowSpace;
            PasswordLength = settings.PasswordLength;
        }
    }
}

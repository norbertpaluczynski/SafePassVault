using System;
using System.Collections.Generic;
using System.Text;

namespace SafePassVault.Core.Models
{
    public class PasswordCharsetPreset
    {
        public string PresetName { get; set; }
        public bool AllowNumbers { get; set; }
        public bool AllowLowercaseLetters { get; set; }
        public bool AllowUppercaseLetters { get; set; }
        public bool AllowSpecialCharacters { get; set; }
        public bool AllowSpace { get; set; }
    }
}

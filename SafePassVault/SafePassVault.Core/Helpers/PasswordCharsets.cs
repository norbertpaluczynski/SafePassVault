using System;
using System.Collections.Generic;
using System.Text;

namespace SafePassVault.Core.Helpers
{
    public static class PasswordCharsets
    {
        public static string Numbers => "0123456789";
        public static string LowercaseLetters => "abcdefghijklmnopqrstuvwxyz";
        public static string UppercaseLetters => "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string SpecialCharacters => "!@#$%^&*()-=_+`~[]{};:\\|,<.>/?\"*";
        public static string Space => " ";
    }
}

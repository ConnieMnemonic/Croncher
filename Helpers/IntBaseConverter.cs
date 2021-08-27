using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Croncher.Helpers
{
    public class IntBaseConverter
    {
        private static readonly string[] BASE_62_ALPHABET = new string[]
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
        };
        private const int BASE_62_RADIX = 62;

        public string Base10ToBase62(int input)
        {
            string result = string.Empty;
            int targetBase = BASE_62_ALPHABET.Length;

            do
            {
                result = BASE_62_ALPHABET[input % targetBase] + result;
                input = input / targetBase;
            }
            while (input > 0);

            return result;
        }

        public int Base62ToBase10(string input)
        {
            int result = 0;
            int multiplier = 1;

            for(int i = input.Length - 1; i >= 0; i--)
            {
                char inputDigit = input[i];
                int digit = Array.IndexOf(BASE_62_ALPHABET, inputDigit);

                result += digit * multiplier;
                multiplier *= BASE_62_RADIX;
            }

            return result;
        }
    }
}

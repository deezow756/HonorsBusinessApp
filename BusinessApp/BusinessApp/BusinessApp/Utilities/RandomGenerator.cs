using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessApp.Utilities
{
    public class RandomGenerator
    {
        private static string[] ranNum = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        private static string[] ranChar = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public static string GenerateNumber(int size)
        {
            string randomString = "";
            for (int i = 0; i < size; i++)
            {
                Random random = new Random();
                randomString += ranNum[random.Next(0, ranNum.Length - 1)];
            }
            return randomString;
        }

        public static string GenerateString(int size)
        {
            string randomString = "";
            for (int i = 0; i < size; i++)
            {
                Random random = new Random();
                randomString += ranChar[random.Next(0, ranChar.Length - 1)];
            }
            return randomString;
        }
    }
}

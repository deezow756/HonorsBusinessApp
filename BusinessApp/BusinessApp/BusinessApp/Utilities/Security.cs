using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BusinessApp.Utilities
{
    public class Security
    {
        public static string EncryptPassword(string pass)
        {
            using (var md5Hash = MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(pass);
                var hashBytes = md5Hash.ComputeHash(sourceBytes);
                var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return hash;              
            }
        }
    }
}

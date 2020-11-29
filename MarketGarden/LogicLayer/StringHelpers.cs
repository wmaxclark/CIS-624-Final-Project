using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public static class StringHelpers
    {
        public static string hashSHA256(this string source)
        {
            string result = "";

            // Byte array to capture the result
            byte[] data;

            // Create a .NET hash provider
            using (SHA256 sha256hash = SHA256.Create())
            {
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }
            // Instantiate a result string builder
            var s = new StringBuilder();

            // Iterate through the data and append to the string builder
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            // Set the result as the string value of the string builder
            result = s.ToString();

            return result;
        }
    }
}

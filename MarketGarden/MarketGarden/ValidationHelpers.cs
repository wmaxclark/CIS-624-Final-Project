using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer
{
    public static class ValidationHelpers
    {
        public static bool isValidPassword(this string password)
        {
            bool result = false;

            // Temporary weak validation
            if (password.Length >= 7) 
            {
                result = true;
            }
            return result;
        }

        public static bool isValidEmail(this string email)
        {
            var result = false;
            if (email.Length > 6
                && email.Length <= 100
                && email.Contains("@")
                && email.Contains("."))
            {
                result = true;
            }
            return result;
        }
    }
}

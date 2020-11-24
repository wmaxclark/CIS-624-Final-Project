using System;
using System.Collections.Generic;
using System.Text;
using DataObjects;

namespace Logic
{
    interface IUserManager
    {
        User AuthenticateUser(string username, string password);

        bool UpdatePassword(string email, string oldPassword, string NewPassword);
    }
}

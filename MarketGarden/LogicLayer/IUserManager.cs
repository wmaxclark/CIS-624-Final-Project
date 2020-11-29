using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayer
{
    public interface IUserManager
    {
        User AuthenticateUser(string userName, string password);

        bool UpdatePassword(string email, string oldPassword, string newPassword);
    }
}

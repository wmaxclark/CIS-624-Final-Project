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
        int CreateUserAccount(string email, string firstName, string lastName, string passwordHash);
        bool UpdatePassword(string email, string oldPassword, string newPassword);
        List<string> GetAllRoles();
        bool CreateUserRole(int userID, Operation operation, string role);
        bool CreateUserRole(int userID, string role);
        bool UpdateUserRole(string email, string role);
    }
}

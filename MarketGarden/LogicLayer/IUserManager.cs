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
        User GetUserByEmail(string email);
        int CreateUserAccount(string email, string firstName, string lastName, string passwordHash);
        bool UpdatePassword(string email, string oldPassword, string newPassword);
        List<string> GetAllRoles();
        bool CreateUserRole(int userID, string role);
        bool CreateUserRole(int userID, string role, Operation operation);
        bool UpdateUserRole(string email, string role);
    }
}

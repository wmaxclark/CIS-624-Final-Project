using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayer
{
    public interface IUserAccessor
    {
        int VerifyEmailAndPassword(string email, string passwordHash);
        List<string> selectRolesByEmail(string email);
        List<string> selectAllRoles();
        User SelectUserByEmail(string email);
        int UpdatePassword(string email, string newPasswordHash, string oldPasswordHash);
        int UpdateUserRole(string email, string role);
        int CreateUserAccount(string email, string firstName, string lastName, string passwordHash);
        int CreateUserRole(int userID, string role);
    }
}

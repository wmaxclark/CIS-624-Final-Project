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
        User SelectUserByEmail(string email);
        int UpdatePassword(string email, string newPasswordHash, string oldPasswordHash);
    }
}

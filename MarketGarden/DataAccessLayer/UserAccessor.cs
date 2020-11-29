using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DataObjects;

namespace DataAccessLayer
{
    public class UserAccessor : IUserAccessor
    {
        public User SelectUserByEmail(string email)
        {
            User user = null;

            var conn = DBConnection.GetDBConnection();
            return user;
        }

        public int UpdatePassword(string email, string newPasswordHash, string oldPasswordHash)
        {
            throw new NotImplementedException();
        }

        public int VerifyEmailAndPassword(string email, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}

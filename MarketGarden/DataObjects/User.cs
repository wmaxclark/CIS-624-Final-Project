using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class User
    {
        public int UserID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public List<string> Roles { get; private set; }

        public User(int userID, string firstName, string lastName,
            string email, List<string> roles)
        {
            this.UserID = userID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Roles = roles;
        }
        public User(int userID, string firstName, string lastName,
            string email)
        {
            this.UserID = userID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }
    }
}

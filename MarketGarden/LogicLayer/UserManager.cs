using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessLayer;
using System.Security.Cryptography;

namespace LogicLayer
{
    public class UserManager : IUserManager
    {
        
        private IUserAccessor userAccessor;

        public UserManager()
        {
            userAccessor = new UserAccessor();
        }

        public User AuthenticateUser(string email, string password)
        {
            User user = null;

            // Hash the password
            password = password.hashSHA256();

            try
            {
                // Only proceed if the data access method returns exactly one result
                if(1 == userAccessor.VerifyEmailAndPassword(email, password))
                {
                    // Get the unique user be their email
                    user = userAccessor.SelectUserByEmail(email);
                }
                else
                {
                    throw new ApplicationException("Incorrect email or password.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Login Failed.", ex);
            }
            return user;
        }

        public List<string> getAllRoles()
        {
            
            List<string> roleList = new List<string>();
            try
            {
                roleList = userAccessor.selectAllRoles();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("No roles found.", ex);
            }
            return roleList;
        }

        public bool UpdatePassword(string email, string oldPassword, string newPassword)
        {
            bool result = false;

            try
            {
                // Hash both passwords
                oldPassword = oldPassword.hashSHA256();
                newPassword = newPassword.hashSHA256();

                // The result is true if the data access method successfully updated one row
                result = (1 == userAccessor.UpdatePassword(email, newPassword, oldPassword));

                // Occurs only if the password was not updated
                if (!result)
                {
                    throw new ApplicationException("Update Failed.");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Password not changed.", ex);
            }
            return result;
        }

        public bool UpdateUserRole(string email, string role)
        {
            bool result = false;

            try
            {
                result = (1 == userAccessor.UpdateUserRole(email, role));
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Role not changed.", ex);
            }
            return result;
        }
        
        public int CreateUserAccount(string email, string firstName, string lastName, string passwordHash)
        {
            int result = 0;
            passwordHash = passwordHash.hashSHA256();
            try
            {
                result = userAccessor.CreateUserAccount(email, firstName, lastName, passwordHash);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Account not created.", ex);
            }
            return result;
        }

        public bool CreateUserRole(int userID, int operationID, string role)
        {
            bool result = false;

            try
            {
                result = (1 == userAccessor.CreateUserRole(userID, operationID, role));
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Role not created.", ex);
            }
            return result;
        }

        public bool CreateUserRole(int userID, string role)
        {
            bool result = false;

            try
            {
                result = (1 == userAccessor.CreateUserRole(userID, role));
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Role not created.", ex);
            }
            return result;
        }
    }
}

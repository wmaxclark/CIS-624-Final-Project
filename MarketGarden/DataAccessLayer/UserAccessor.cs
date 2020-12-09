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

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_user_by_email", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);

            // Set parameter to value
            cmd.Parameters["@Email"].Value = email;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Execute command
                var reader = cmd.ExecuteReader();

                // Capture results
                if (reader.HasRows)
                {
                    reader.Read();
                    var userID = reader.GetInt32(0);
                    var firstName = reader.GetString(2);
                    var lastName = reader.GetString(3);
                    var active = reader.GetBoolean(4);
                    reader.Close();

                    // Get roles from another stored procedure
                    List<string> roles = selectRolesByEmail(email);

                    // Construct new user with captured values
                    user = new User(userID, firstName, lastName, email, roles);
                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return user;
        }

        public List<string> selectRolesByEmail(string email)
        {
            List<string> roles = new List<string>();

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_user_role_by_email", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);

            // Set parameter to value
            cmd.Parameters["@Email"].Value = email;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Execute command
                var reader = cmd.ExecuteReader();

                // Capture results
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return roles;
        }

        public int UpdatePassword(string email, string newPasswordHash, string oldPasswordHash)
        {
            // Result of verification representing rows matched, success will mean a result of 1
            int result = 0;

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_update_passwordhash", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);

            // Add parameter to command
            cmd.Parameters.Add("@newPasswordHash", SqlDbType.NVarChar, 100);

            // Add parameter to command
            cmd.Parameters.Add("@oldPasswordHash", SqlDbType.NVarChar, 100);

            // Set parameter to value
            cmd.Parameters["@Email"].Value = email;

            // Set parameter to value
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;

            // Set parameter to value
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Capture result of the execution
                result = Convert.ToInt32(cmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public int VerifyEmailAndPassword(string email, string passwordHash)
        {
            // Result of verification representing rows matched, success will mean a result of 1
            int result = 0;

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_authenticate_user", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);

            // Add parameter to command
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            // Set parameter to value
            cmd.Parameters["@Email"].Value = email;

            // Set parameter to value
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Capture result of the execution
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public List<string> selectAllRoles()
        {
            List<string> roles = new List<string>();

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_all_role", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Execute command
                var reader = cmd.ExecuteReader();

                // Capture results
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return roles;
        }

        public int UpdateUserRole(string email, string role)
        {
            // Result of verification representing rows matched, success will mean a result of 1
            int result = 0;

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_update_user_role_by_email", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);

            // Add parameter to command
            cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 100);

            // Set parameter to value
            cmd.Parameters["@Email"].Value = email;

            // Set parameter to value
            cmd.Parameters["@RoleName"].Value = role;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Capture result of the execution
                result = Convert.ToInt32(cmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public int CreateUserAccount(string email, string firstName, string lastName, string passwordHash)
        {
            // Result of verification representing rows matched, success will mean a result of 1
            int result = 0;

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_update_user_role_by_email", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);

            // Add parameter to command
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 100);

            // Add parameter to command
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 100);

            // Add parameter to command
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            // Set parameter to value
            cmd.Parameters["@Email"].Value = email;

            // Set parameter to value
            cmd.Parameters["@FirstName"].Value = firstName;

            // Set parameter to value
            cmd.Parameters["@LastName"].Value = lastName;

            // Set parameter to value
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Capture result of the execution
                result = Convert.ToInt32(cmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public int CreateUserRole(int userID, string role)
        {
            // Result of verification representing rows matched, success will mean a result of 1
            int result = 0;

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_update_user_role_by_email", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 100);

            // Add parameter to command
            cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 100);

            // Set parameter to value
            cmd.Parameters["@UserID"].Value = userID;

            // Set parameter to value
            cmd.Parameters["@RoleName"].Value = role;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Capture result of the execution
                result = Convert.ToInt32(cmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
    }
}

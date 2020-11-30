using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class OperationAccessor : IOperationAccessor
    {
        public Operation RetrieveOperationByOperator(User operatorUser)
        {
            Operation operation = null;

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_farmoperation_by_operator", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@UserID_Operator", SqlDbType.NVarChar, 100);

            // Set parameter to value
            cmd.Parameters["@UserID_Operator"].Value = operatorUser.UserID;

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
                    var operationID = reader.GetInt32(0);
                    var operationName = reader.GetString(2);
                    var zipCode = reader.GetInt32(3);
                    var maxShares = reader.GetInt32(4);
                    var active = reader.GetBoolean(5);
                    reader.Close();

                    // Get roles from another stored procedure
                    List<string> helpers = RetrieveHelpersByOperation();

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
            return operation;
        }

        public List<Product> RetrieveProductsByOperation(int operationID)
        {
            throw new NotImplementedException();
        }
    }
}

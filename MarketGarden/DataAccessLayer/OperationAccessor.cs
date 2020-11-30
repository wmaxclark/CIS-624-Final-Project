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

                    // Get helpers from another stored procedure
                    List<User> helpers = RetrieveHelpersByOperation(operationID);

                    // Get products from another stored procedure
                    List<Product> products = RetrieveProductsByOperation(operationID);

                    // Get tasks from another stored procedure
                    List<UserTask> tasks = RetrieveTasksByOperation(operationID);

                    // Construct new operation with captured values
                    operation = new Operation(operationID, operatorUser, operationName, zipCode, maxShares, active, helpers, products, tasks);
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

        public List<User> RetrieveHelpersByOperation(int operationID)
        {
            throw new NotImplementedException();
        }

        public List<UserTask> RetrieveTasksByOperation(int operationID)
        {
            throw new NotImplementedException();
        }
    }
}

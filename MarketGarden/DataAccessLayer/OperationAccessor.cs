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
            cmd.Parameters.Add("@UserID_Operator", SqlDbType.Int);

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
                    var operationName = reader.GetString(1);
                    var zipCode = reader.GetInt32(2);
                    var active = reader.GetBoolean(4);
                    reader.Close();

                    // Get helpers from another stored procedure
                    List<User> helpers = RetrieveHelpersByOperation(operationID);

                    // Get products from another stored procedure
                    List<Product> products = RetrieveProductsByOperation(operationID);

                    // Get tasks from another stored procedure
                    List<UserTask> tasks = RetrieveTasksBySender(operatorUser);

                    // Construct new operation with captured values
                    operation = new Operation(operationID, operatorUser, operationName, zipCode, active, helpers, products, tasks);
                }
                else
                {
                    throw new ApplicationException("Operation not found.");
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
            List<Product> productList = new List<Product>();

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_product_by_operation", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@OperationID", SqlDbType.Int);

            // Set parameter to value
            cmd.Parameters["@OperationID"].Value = operationID;

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
                        var productID = reader.GetInt32(0);
                        var productName = reader.GetString(1);
                        var productDescription = reader.GetString(2);
                        var inputCost = reader.GetDecimal(3);
                        var unit = reader.GetString(4);
                        var unitPrice = reader.GetDecimal(5);
                        var germinationDate = reader.GetSqlDateTime(6);
                        var daysAfterGerminationToPlant = reader.GetInt32(7);
                        var daysAfterGerminationToTransplant = reader.GetInt32(8);
                        var daysAfterGerminationToHarvest = reader.GetInt32(9);
                        

                        // Construct new operation with captured values
                        var product = new Product(productID, operationID, productName, productDescription,
                            unit, inputCost, unitPrice, (DateTime)germinationDate,
                            daysAfterGerminationToPlant,
                            daysAfterGerminationToTransplant,
                            daysAfterGerminationToHarvest);

                        // Add the resulting product to the list
                        productList.Add(product);
                    }
                    reader.Close();
                }
                else
                {
                    throw new ApplicationException("No products found.");
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
            return productList;
        }

        public List<User> RetrieveHelpersByOperation(int operationID)
        {
            List<User> helperList = new List<User>();

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_user_role_by_operation", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@OperationID", SqlDbType.Int);

            // Set parameter to value
            cmd.Parameters["@OperationID"].Value = operationID;

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
                        var userID = reader.GetInt32(0);
                        var firstName = reader.GetString(1);
                        var lastName = reader.GetString(2);
                        var email = reader.GetString(3);
                        var role = reader.GetString(4);
                        // Instantiate a list of roles the user has by calling the stored procedure
                        //List<string> roles = RetrieveRolesByEmail(email);

                        // Validate that the user has the role of a helper
                        if (role.Equals("Helper"))
                        {
                            // Construct new user with captured values
                            User helper = new User(userID, firstName, lastName, email);
                            // Add the resulting user to the list
                            helperList.Add(helper);
                        }
                        
                    }
                    reader.Close();
                }
                else
                {
                    throw new ApplicationException("No products found.");
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
            return helperList;
        }

        public List<UserTask> RetrieveTasksBySender(User operatorUser)
        {
            List<UserTask> taskList = new List<UserTask>();

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_task_by_sender", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@UserID_Sender", SqlDbType.Int);

            // Set parameter to value
            cmd.Parameters["@UserID_Sender"].Value = operatorUser.UserID;

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
                        var userID_assignee = reader.GetInt32(0);
                        var assignDate = reader.GetSqlDateTime(1);
                        var dueDate = reader.GetSqlDateTime(2);
                        var taskName = reader.GetString(3);
                        var taskDescription = reader.GetString(3);
                        var finished = reader.GetBoolean(4);
                        reader.Close();

                        User assignee = RetrieveUserByID(userID_assignee);

                        // Construct new task with captured values
                        UserTask task = new UserTask(operatorUser, assignee, (DateTime)assignDate,
                            (DateTime)dueDate, taskName, taskDescription);

                        // Add the resulting task to the list
                        taskList.Add(task);

                    }
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
            return taskList;
        }

        public List<string> RetrieveRolesByEmail(string email)
        {
            List<string> roleList = new List<string>();

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
                        var role = reader.GetString(0);

                        // Add the resulting task to the list
                        roleList.Add(role);
                    }
                    reader.Close();
                }
                else
                {
                    throw new ApplicationException("No products found.");
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
            return roleList;
        }

        public User RetrieveUserByID(int userID_assignee)
        {
            throw new NotImplementedException();
        }
    }
}

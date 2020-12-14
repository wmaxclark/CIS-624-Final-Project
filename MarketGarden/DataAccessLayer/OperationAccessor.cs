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
                    var addressState = reader.GetString(2);

                    /**
                     * Solution credited to user Stefan Hoffman, found at:
                     * https://social.msdn.microsoft.com/Forums/en-US/69a113aa-fadf-44bb-a090-5156a33e85d7/how-to-read-null-values-in-sql-table-column-readergetint32-c?forum=adodotnetdataproviders
                     */
                    int? maxShares = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                    var active = reader.GetBoolean(4);
                    reader.Close();

                    // Construct new operation with captured values
                    operation = new Operation(operationID, operatorUser.UserID, operationName, addressState, maxShares, active);
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
                        var plantDate = reader.GetSqlDateTime(7);
                        var transplantDate = reader.GetSqlDateTime(8);
                        var harvestDate = reader.GetSqlDateTime(9);
                        

                        // Construct new operation with captured values
                        var product = new Product(productID, operationID, productName, 
                            productDescription, unit, inputCost, 
                            unitPrice, (DateTime)germinationDate,
                            (DateTime)plantDate,
                            (DateTime)transplantDate,
                            (DateTime)harvestDate);

                        // Add the resulting product to the list
                        productList.Add(product);
                    }
                    reader.Close();
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

        public int CreateOperation(int userID_operator, string state, string operationName)
        {
            int result = 0;

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_create_farmoperation", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@UserID_Operator", SqlDbType.Int);

            // Add parameter to command
            cmd.Parameters.Add("@AddressState", SqlDbType.Char, 2);

            // Add parameter to command
            cmd.Parameters.Add("@OperationName", SqlDbType.NVarChar, 64);

            // Set parameter to value
            cmd.Parameters["@UserID_Operator"].Value = userID_operator;

            // Set parameter to value
            cmd.Parameters["@AddressState"].Value = state;

            // Set parameter to value
            cmd.Parameters["@OperationName"].Value = operationName;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Execute command
                result = cmd.ExecuteNonQuery();

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

        public List<string> RetrieveAllStates()
        {
            List<string> states = new List<string>();

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_all_address", conn);

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
                        var role = reader.GetString(0);

                        // Add the resulting task to the list
                        states.Add(role);
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
            return states;
        }

        public List<Operation> RetrieveAllOperations()
        {
            List<Operation> operationList = new List<Operation>();

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_all_farmoperation", conn);

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
                    reader.Read();
                    var operationID = reader.GetInt32(0);
                    var operationName = reader.GetString(1);
                    var userID_Operator = reader.GetInt32(2);
                    var addressState = reader.GetString(3);
                    /**
                     * Solution credited to user Stefan Hoffman, found at:
                     * https://social.msdn.microsoft.com/Forums/en-US/69a113aa-fadf-44bb-a090-5156a33e85d7/how-to-read-null-values-in-sql-table-column-readergetint32-c?forum=adodotnetdataproviders
                     */
                    int? maxShares = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);
                    var active = reader.GetBoolean(5);
                    reader.Close();

                    // Construct new operation with captured values
                    var operation = new Operation(operationID, userID_Operator, operationName, addressState, maxShares, active);

                    operationList.Add(operation);
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
            return operationList;
        }

        public int CreateProduct(int operationID, string productName, string productDescription, string unit, decimal inputCost, decimal unitPrice, 
            DateTime germinationDate, DateTime plantDate, DateTime transplantDate, DateTime harvestDate)
        {
            int result = 0;

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_create_product", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@OperationID", SqlDbType.Int);

            // Add parameter to command
            cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 64);

            // Add parameter to command
            cmd.Parameters.Add("@ProductDescription", SqlDbType.NVarChar, 1024);

            // Add parameter to command
            cmd.Parameters.Add("@InputCost", SqlDbType.Decimal);

            // Add parameter to command
            cmd.Parameters.Add("@Unit", SqlDbType.NVarChar, 64);

            // Add parameter to command
            cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal);

            // Add parameter to command
            cmd.Parameters.Add("@GerminationDate", SqlDbType.DateTime);

            // Add parameter to command
            cmd.Parameters.Add("@PlantDate", SqlDbType.DateTime);

            // Add parameter to command
            cmd.Parameters.Add("@TransplantDate", SqlDbType.DateTime);

            // Add parameter to command
            cmd.Parameters.Add("@HarvestDate", SqlDbType.DateTime);

            // Set parameter to value
            cmd.Parameters["@OperationID"].Value = operationID;

            // Set parameter to value
            cmd.Parameters["@ProductName"].Value = productName;

            // Set parameter to value
            cmd.Parameters["@ProductDescription"].Value = productDescription;

            // Set parameter to value
            cmd.Parameters["@InputCost"].Value = inputCost;

            // Set parameter to value
            cmd.Parameters["@Unit"].Value = unit;

            // Set parameter to value
            cmd.Parameters["@UnitPrice"].Value = unitPrice;

            // Set parameter to value
            cmd.Parameters["@GerminationDate"].Value = germinationDate;

            // Set parameter to value
            cmd.Parameters["@PlantDate"].Value = plantDate;

            // Set parameter to value
            cmd.Parameters["@TransplantDate"].Value = transplantDate;

            // Set parameter to value
            cmd.Parameters["@HarvestDate"].Value = harvestDate;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Execute command
                result = cmd.ExecuteNonQuery();

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

        public int UpdateProduct(int operationID, Product oldProduct, string productName, string productDescription, string unit, decimal inputCost, decimal unitPrice, DateTime germinationDate, DateTime plantDate, DateTime transplantDate, DateTime harvestDate)
        {
            int result = 0;

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_update_product", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@ProductID", SqlDbType.Int);

            // Add parameter to command
            cmd.Parameters.Add("@OperationID", SqlDbType.Int);

            // Add parameter to command
            cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 64);

            // Add parameter to command
            cmd.Parameters.Add("@ProductDescription", SqlDbType.NVarChar, 1024);

            // Add parameter to command
            cmd.Parameters.Add("@InputCost", SqlDbType.Decimal);

            // Add parameter to command
            cmd.Parameters.Add("@Unit", SqlDbType.NVarChar, 64);

            // Add parameter to command
            cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal);

            // Add parameter to command
            cmd.Parameters.Add("@GerminationDate", SqlDbType.DateTime);

            // Add parameter to command
            cmd.Parameters.Add("@PlantDate", SqlDbType.DateTime);

            // Add parameter to command
            cmd.Parameters.Add("@TransplantDate", SqlDbType.DateTime);

            // Add parameter to command
            cmd.Parameters.Add("@HarvestDate", SqlDbType.DateTime);

            // Set parameter to value
            cmd.Parameters["@ProductID"].Value = oldProduct.ProductID;

            // Set parameter to value
            cmd.Parameters["@OperationID"].Value = operationID;

            // Set parameter to value
            cmd.Parameters["@ProductName"].Value = productName;

            // Set parameter to value
            cmd.Parameters["@ProductDescription"].Value = productDescription;

            // Set parameter to value
            cmd.Parameters["@InputCost"].Value = inputCost;

            // Set parameter to value
            cmd.Parameters["@Unit"].Value = unit;

            // Set parameter to value
            cmd.Parameters["@UnitPrice"].Value = unitPrice;

            // Set parameter to value
            cmd.Parameters["@GerminationDate"].Value = germinationDate;

            // Set parameter to value
            cmd.Parameters["@PlantDate"].Value = plantDate;

            // Set parameter to value
            cmd.Parameters["@TransplantDate"].Value = transplantDate;

            // Set parameter to value
            cmd.Parameters["@HarvestDate"].Value = harvestDate;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Execute command
                result = cmd.ExecuteNonQuery();

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

        public int DeleteProduct(int productID)
        {
            int result = 0;
            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_delete_product", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@ProductID", SqlDbType.Int);

            // Set parameter to value
            cmd.Parameters["@ProductID"].Value = productID;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Execute command
                result = cmd.ExecuteNonQuery();
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

        public List<Order> RetrieveOrdersByOperation(int operationID)
        {
            List<Order> orderList = new List<Order>();

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_productorder_by_operation", conn);

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
                        var orderID = reader.GetInt32(0);
                        var userID_Customer = reader.GetInt32(1);
                        var orderDate = reader.GetDateTime(2);

                        var orderLines = RetrieveOrderLinesByOrder(orderID);

                        var order = new Order(orderID, operationID, userID_Customer, orderDate, orderLines);

                        // Add the resulting order to the list
                        orderList.Add(order);
                    }
                    reader.Close();
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
            return orderList;
        }

        public List<WeeklyShare> RetrieveWeeklySharesByOperation(int operationID)
        {
            List<WeeklyShare> weeklyShares = new List<WeeklyShare>();

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_productorder_by_operation", conn);

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
                        var userID_Customer = reader.GetInt32(0);
                        var sharePortion = reader.GetDecimal(1);
                        var frequency = reader.GetInt32(2);

                        var share = new WeeklyShare(operationID, userID_Customer, sharePortion, frequency);

                        // Add the resulting order to the list
                        weeklyShares.Add(share);
                    }
                    reader.Close();
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
            return weeklyShares;
        }

        public List<OrderLine> RetrieveOrderLinesByOrder(int orderID)
        {
            List<OrderLine> lineList = new List<OrderLine>();

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_select_productorder_by_operation", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@OrderID", SqlDbType.Int);

            // Set parameter to value
            cmd.Parameters["@OrderID"].Value = orderID;

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
                        var orderLineID = reader.GetInt32(1);
                        var priceCharged = reader.GetDecimal(2);

                        var line = new OrderLine(orderID, productID, orderLineID, priceCharged);

                        // Add the resulting order to the list
                        lineList.Add(line);
                    }
                    reader.Close();
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
            return lineList;
        }

        public int CreateOrder(int userID, int operationID, DateTime now)
        {
            int result = 0;

            // Retrieve a connection from factory
            var conn = DBConnection.GetDBConnection();

            // Retrieve a command
            var cmd = new SqlCommand("sp_create_farmoperation", conn);

            // Set command type to stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameter to command
            cmd.Parameters.Add("@UserID_Customer", SqlDbType.Int);

            // Add parameter to command
            cmd.Parameters.Add("@OperationID", SqlDbType.Int);

            // Add parameter to command
            cmd.Parameters.Add("@OrderDate", SqlDbType.DateTime);

            // Set parameter to value
            cmd.Parameters["@UserID_Customer"].Value = userID;

            // Set parameter to value
            cmd.Parameters["@OperationID"].Value = operationID;

            // Set parameter to value
            cmd.Parameters["@OrderDate"].Value = now;

            // Execute command
            try
            {
                // Open connection
                conn.Open();

                // Execute command
                result = cmd.ExecuteNonQuery();

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

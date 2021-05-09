using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.ComponentModel;

namespace LogicLayer
{
    public class OperationManager : IOperationManager
    {
        private IOperationAccessor _operationAccessor;

        public OperationManager()
        {
            _operationAccessor = new OperationAccessor();
        }
        public int CreateOperation(int userID_operator, string state, string operationName)
        {
            int result = 0;

            try
            {
                result =  _operationAccessor.CreateOperation(userID_operator, state, operationName);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Operation not created.", ex);
            }
            return result;
        }
        public Operation GetOperationByOperator(User operatorUser)
        {
            Operation operation = null;

            try
            {
                operation = _operationAccessor.RetrieveOperationByOperator(operatorUser);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Operation not available", ex);
            }
            return operation;
        }
        public BindingList<Operation> GetAllOperations()
        {
            BindingList<Operation> operations = new BindingList<Operation>();

            try
            {
                operations = _operationAccessor.RetrieveAllOperations();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Operations not available", ex);
            }
            return operations;
        }
        public OperationVM GetOperationVMByOperator(User user)
        {
            OperationVM operationVM;

            try
            {
                Operation operation = _operationAccessor.RetrieveOperationByOperator(user);
                operationVM = new OperationVM(operation.OperationID, user, operation.OperationName,
                    operation.AddressState, operation.MaxShares, operation.Active,
                    _operationAccessor.RetrieveProductsByOperation(operation.OperationID),
                    _operationAccessor.RetrieveOrdersByOperation(operation.OperationID),
                    _operationAccessor.RetrieveWeeklySharesByOperation(operation.OperationID));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Operation not available", ex);
            }
            return operationVM;
        }
        public bool AddProduct(int operationID, string productName, string productDescription, 
            string unit, decimal inputCost, decimal unitPrice, DateTime germinationDate,
            DateTime plantDate, DateTime transplantDate, DateTime harvestDate)
        {
            bool result = false;

            try
            {
                result = (1 == _operationAccessor.CreateProduct(operationID, productName, productDescription, unit,
                    inputCost, unitPrice, germinationDate, plantDate,
                    transplantDate, harvestDate));
               
            }
            catch (Exception)
            {

                throw new ApplicationException("Product could not be created.");
            }

            return result;
        }
        public bool UpdateProduct(int operationID, Product oldProduct, string productName, string productDescription, 
            string unit, decimal inputCost, decimal unitPrice, DateTime germinationDate,
            DateTime plantDate, DateTime transplantDate, DateTime harvestDate)
        {
            bool result = false;
            try
            {
                result = (1 == _operationAccessor.UpdateProduct(operationID, oldProduct, productName, productDescription, unit,
                    inputCost, unitPrice, germinationDate, plantDate,
                    transplantDate, harvestDate));
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Product could not be updated." + ex.InnerException.Message);
            }
            return result;
        }
        public bool CloneProduct(Product oldProduct, int operationID, string productName, string productDescription, string unit, decimal inputCost, decimal unitPrice, DateTime germinationDate)
        {
            bool result = false;
            try
            {
                // Calculate the interval between the germination date of the original product
                TimeSpan timeSpan = oldProduct.PlantDate - oldProduct.GerminationDate;

                // Add the interval to the date
                DateTime newPlantDate = germinationDate.Add(timeSpan);

                // Calculate the interval between the germination date of the original product
                timeSpan = oldProduct.TransplantDate - oldProduct.GerminationDate;

                // Add the interval to the date
                DateTime newTransplantDate = germinationDate.Add(timeSpan);

                // Calculate the interval between the germination date of the original product
                timeSpan = oldProduct.HarvestDate - oldProduct.GerminationDate;

                // Add the interval to the date
                DateTime newHarvestDate = germinationDate.Add(timeSpan);
                result = AddProduct(operationID, productName, productDescription, unit, inputCost, unitPrice, germinationDate, newPlantDate, newTransplantDate, newHarvestDate);
            }
            catch (Exception)
            {
                throw new ApplicationException("Product could not be created.");
            }
            return result;
        }
        public List<Product> GetProductsByOperation(int operationID)
        {
            List<Product> products = new List<Product>();
            try
            {
                products = _operationAccessor.RetrieveProductsByOperation(operationID);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Product list could not found." + ex.InnerException.Message);
            }
            return products;
        }
        public List<Product> RefreshProductList(OperationVM operation)
        {
            List<Product> products = new List<Product>();
            try
            {
                products = _operationAccessor.RetrieveProductsByOperation(operation.OperationID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Product list could not be refreshed." + ex.InnerException.Message);
            }
            return products;
        }
        public bool DeleteProduct(Product product)
        {
            bool result = false;
            try
            {
                result = (1 == _operationAccessor.DeleteProduct(product.ProductID));
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Product could not be deleted." + ex.InnerException.Message);
            }
            return result;
        }
        public List<string> GetAllStates()
        {
            List<string> states = new List<string>();

            try
            {
                states = _operationAccessor.RetrieveAllStates();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("States not available", ex);
            }
            return states;
        }
        public List<WeeklyShare> RefreshWeeklyShares(OperationVM operation)
        {
            List<WeeklyShare> weeklyShares = new List<WeeklyShare>();
            try
            {
                weeklyShares = _operationAccessor.RetrieveWeeklySharesByOperation(operation.OperationID);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Weekly share list could not be refreshed." + ex.InnerException.Message);
            }
            return weeklyShares;
        }
        public List<Order> RefreshOrderList(OperationVM operation)
        {
            List<Order> orders = new List<Order>();
            try
            {
                orders = _operationAccessor.RetrieveOrdersByOperation(operation.OperationID);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Weekly share list could not be refreshed." + ex.InnerException.Message);
            }
            return orders;
        }
        public bool CreateOrder(User user, int operationID, DateTime now, BindingList<Product> productList)
        {
            bool result = false;
            try
            {
                result = (1 == _operationAccessor.CreateOrder(user.UserID, operationID, now, productList));
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Order could not be created." + ex.InnerException.Message);
            }
            return result;
        }
        public BindingList<Order> GetOrderListByUser(User user)
        {
            BindingList<Order> orders = new BindingList<Order>();
            try
            {
                orders = _operationAccessor.RetrieveOrdersByCustomer(user.UserID);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Weekly share list could not be refreshed." + ex.InnerException.Message);
            }
            return orders;
        }
        public bool CreateWeeklyShare(User user, int operationID, decimal v1, int v2)
        {
            bool result = false;
            try
            {
                result = (0 != _operationAccessor.CreateWeeklyShare(user.UserID, operationID, v1, v2));
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Weekly share could not be created." + ex.InnerException.Message);
            }
            return result;
        }
        public bool GetWeeklyShareByUser(User user, int operationID)
        {
            bool isSubscribed = false;
            try
            {
                foreach (var subscription in _operationAccessor.GetWeeklyShareByCustomer(user.UserID))
                {
                    if (subscription.OperationID == operationID)
                    {
                        isSubscribed = true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Weekly share could not be created." + ex.InnerException.Message);
            }
            return isSubscribed;
        }
        
    }
}

using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

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
        public List<Operation> GetAllOperations()
        {
            List<Operation> operations = new List<Operation>();

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
                    _operationAccessor.RetrieveHelpersByOperation(operation.OperationID),
                    _operationAccessor.RetrieveProductsByOperation(operation.OperationID),
                    _operationAccessor.RetrieveTasksBySender(user));
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
        public bool AddTask(UserTask userTask)
        {
            throw new NotImplementedException();
        }
        public bool FinishTask(UserTask userTask)
        {
            throw new NotImplementedException();
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

        
    }
}

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
        public bool AddProduct(Product product)
        {
            throw new NotImplementedException();
        }
        public List<Product> RetrieveProductsByOperation(Operation operation)
        {
            List<Product> products = null;
            try
            {
                products = _operationAccessor.RetrieveProductsByOperation(operation);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Product list not available", ex);
            }
            return products;
        }
        public bool DeleteProduct(Product product)
        {
            throw new NotImplementedException();
        }
        public bool AddTask(UserTask userTask)
        {
            throw new NotImplementedException();
        }
        public bool FinishTask(UserTask userTask)
        {
            throw new NotImplementedException();
        }
        
    }
}

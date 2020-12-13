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
        public int CreateOperation(int userID_operator, int zipCode, string operationName)
        {
            int result = 0;

            try
            {
                result =  _operationAccessor.CreateOperation(userID_operator, zipCode, operationName);
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
        public bool AddProduct(Product product)
        {
            throw new NotImplementedException();
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

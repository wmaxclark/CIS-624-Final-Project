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

        public List<string> getAllStates()
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

        public List<Operation> getAllOperations()
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
    }
}

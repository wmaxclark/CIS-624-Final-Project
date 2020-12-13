using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayer
{
    public interface IOperationManager
    {
        int CreateOperation(int userID_operator, string state, string operationName);
        Operation GetOperationByOperator(User operatorUser);
        bool AddProduct(Product product);
        //List<Product> RetrieveProductsByOperation(Operation operation);
        bool DeleteProduct(Product product);
        bool AddTask(UserTask userTask);
        bool FinishTask(UserTask userTask);
        List<string> getAllStates();
        List<Operation> getAllOperations();
        OperationVM GetOperationVMByOperator(User user);
    }
}

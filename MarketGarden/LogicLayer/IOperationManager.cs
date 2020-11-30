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
        Operation GetOperationByOperator(User operatorUser);
        bool AddProduct(Product product);
        List<Product> RetrieveProductsByOperation(Operation operation);
        bool DeleteProduct(Product product);
        bool AddTask(UserTask userTask);
        bool FinishTask(UserTask userTask);
    }
}

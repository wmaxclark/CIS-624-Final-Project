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
        Operation getOperationByOperator(User operatorUser);
        bool addProduct(Product product);
        bool deleteProduct(Product product);
        bool addTask(UserTask userTask);
        bool finishTask(UserTask userTask);
    }
}

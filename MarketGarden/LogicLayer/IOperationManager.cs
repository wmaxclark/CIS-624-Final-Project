using System;
using System.Collections;
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
        bool DeleteProduct(Product product);
        List<string> GetAllStates();
        List<Operation> GetAllOperations();
        OperationVM GetOperationVMByOperator(User user);
        bool AddProduct(int operationID, string productName, string productDescription, string unit, decimal inputCost, decimal unitPrice,
            DateTime germinationDate, DateTime plantDate, DateTime transplantDate, DateTime harvestDate);
        bool UpdateProduct(int operationID, Product oldProduct, string productName, string productDescription, string unit, decimal inputCost, decimal unitPrice, 
            DateTime germinationDate, DateTime plantDate, DateTime transplantDate, DateTime harvestDate);
        List<Product> GetProductsByOperation(int operationID);
        List<Product> RefreshProductList(OperationVM operation);
        List<WeeklyShare> RefreshWeeklyShares(OperationVM operation);
    }
}

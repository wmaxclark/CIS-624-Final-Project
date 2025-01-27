﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        BindingList<Operation> GetAllOperations();
        OperationViewModel GetOperationVMByOperator(User user);
        bool AddProduct(int operationID, string productName, string productDescription, string unit, decimal inputCost, decimal unitPrice,
            DateTime germinationDate, DateTime plantDate, DateTime transplantDate, DateTime harvestDate);
        OperationViewModel GetOperationViewModelByOperation(Operation operation);
        bool UpdateProduct(int operationID, Product oldProduct, string productName, string productDescription, string unit, decimal inputCost, decimal unitPrice, 
            DateTime germinationDate, DateTime plantDate, DateTime transplantDate, DateTime harvestDate);
        bool CloneProduct(Product oldProduct, int operationID, string productName, string productDescription, string unit, decimal inputCost, decimal unitPrice, DateTime germinationDate);
        List<Product> GetProductsByOperation(int operationID);
        List<Product> RefreshProductList(OperationViewModel operation);
        List<WeeklyShare> RefreshWeeklyShares(OperationViewModel operation);
        List<Order> RefreshOrderList(OperationViewModel operation);
        bool CreateOrder(User user, int operationID, DateTime now, BindingList<Product> productList);
        BindingList<Order> GetOrderListByUser(User _user);
        bool CreateWeeklyShare(User user, int operationID, decimal v1, int v2);
        bool GetWeeklyShareByUser(User user, int operationID);
        Product GetProductByProductID(int v);
    }
}

﻿using DataObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IOperationAccessor
    {
        int CreateOperation(int userID_operator, string state, string operationName);
        Operation RetrieveOperationByOperator(User operatorUser);
        List<Product> RetrieveProductsByOperation(int operationID);
        List<string> RetrieveRolesByEmail(string email);
        List<string> RetrieveAllStates();
        BindingList<Operation> RetrieveAllOperations();
        int CreateProduct(int operationID, string productName, string productDescription, string unit, decimal inputCost, decimal unitPrice, 
            DateTime germinationDate, DateTime plantDate, DateTime transplantDate, DateTime harvestDate);
        int UpdateProduct(int operationID, Product oldProduct, string productName, string productDescription, string unit, decimal inputCost, decimal unitPrice, 
            DateTime germinationDate, DateTime plantDate, DateTime transplantDate, DateTime harvestDate);
        int DeleteProduct(int productID);
        List<Order> RetrieveOrdersByOperation(int operationID);
        List<OrderLine> RetrieveOrderLinesByOrder(int orderID);
        List<WeeklyShare> RetrieveWeeklySharesByOperation(int operationID);
        int CreateOrder(int userID, int operationID, DateTime now, BindingList<Product> products);
        BindingList<Order> RetrieveOrdersByCustomer(int userID);
        int CreateWeeklyShare(int userID, int operationID, decimal v1, int v2);
        List<WeeklyShare> GetWeeklyShareByCustomer(int userID);
        Product SelectProductByProductId(int productId);
    }
}

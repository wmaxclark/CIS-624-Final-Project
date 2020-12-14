using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Order
    {
        int OrderID { get; set; }
        int OperationID { get; set; }
        int CustomerID { get; set; }
        DateTime OrderDate { get; set; }
        List<OrderLine> Lines { get; set; }

        public Order(int orderID, int operationID, int customerID, DateTime orderDate, List<OrderLine> lines)
        {
            OrderID = orderID;
            OperationID = operationID;
            CustomerID = customerID;
            OrderDate = orderDate;
            Lines = lines;
        }
    }
}

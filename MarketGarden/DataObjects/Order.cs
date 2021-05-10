using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [Display(Name = "Name")]
        public DateTime OrderDate { get; set; }
        [Required]
        [Display(Name = "Lines")]
        public List<OrderLine> Lines { get; set; }

        public Order(int orderID, int operationID, int customerID, DateTime orderDate, List<OrderLine> lines)
        {
            OrderID = orderID;
            OperationID = operationID;
            CustomerID = customerID;
            OrderDate = orderDate;
            Lines = lines;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in Lines)
            {
                stringBuilder.Append(item.ToString());
            }
            return OrderDate.ToString() + " " + stringBuilder.ToString() ;
        }
    }
}

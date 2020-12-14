using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class OperationVM : Operation
    {

        public User Operator { get;  set; }

        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
        public List<WeeklyShare> WeeklyShares{ get; set;}

        public OperationVM(int operationID, User operatorUser, string operationName, string addressState, int? maxShares, bool active, List<Product> products, List<Order> orders, List<WeeklyShare> weeklyShares) : base(operationID, operatorUser.UserID, operationName, addressState, maxShares, active)
        {
            this.Operator = operatorUser;
            this.Products = products;
            this.Orders = orders;
            this.WeeklyShares = weeklyShares;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

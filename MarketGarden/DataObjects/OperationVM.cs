using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class OperationVM : Operation
    {

        public User Operator { get; private set; }
        public List<User> Helpers { get; set; }

        public List<Product> Products { get; set; }
        public List<UserTask> Tasks { get; set; }

        public OperationVM(int operationID, User operatorUser, string operationName, string addressState, int? maxShares, bool active, List<User> helpers, List<Product> products, List<UserTask> userTasks) : base(operationID, operatorUser.UserID, operationName, addressState, maxShares, active)
        {
            this.Operator = operatorUser;
            this.Helpers = helpers;
            this.Products = products;
            this.Tasks = userTasks;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Operation
    {
        public int OperationID { get; private set; }
        public User Operator { get; private set; }
        public string OperationName { get; private set; }
        public int Zipcode { get; private set; }
        public int MaxShares { get; private set; }
        public bool Active { get; private set; }
        public List<User> Helpers { get; private set; }

        public List<Product> Products { get; private set;}
        public List<UserTask> Tasks { get; private set; }

        public Operation(int operationID, User operatorUser, 
            string operationName, int zipcode,
            bool active, List<User> helpers, 
            List<Product> products, List<UserTask> userTasks)
        {
            this.OperationID = operationID;
            this.Operator = operatorUser;
            this.OperationName = operationName;
            this.Zipcode = zipcode;
            this.Active = active;
            this.Helpers = helpers;
            this.Products = products;
            this.Tasks = userTasks;
        }
    }
}

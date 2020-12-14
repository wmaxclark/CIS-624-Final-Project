using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Operation
    {
        public int OperationID { get;  set; }
        public int UserID_Operator { get;  set; }
        public string OperationName { get;  set; }
        public string AddressState { get;  set; }
        public int? MaxShares { get;  set; }
        public bool Active { get;  set; }
        

        public Operation(int operationID, int userID_Operator, 
            string operationName, string addressState, int? maxShares,
            bool active)
        {
            this.OperationID = operationID;
            this.UserID_Operator = userID_Operator;
            this.OperationName = operationName;
            this.AddressState = addressState;
            this.MaxShares = maxShares;
            this.Active = active;
        }

        public override string ToString()
        {
            return OperationName;
        }
    }
}

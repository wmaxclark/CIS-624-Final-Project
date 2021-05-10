using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class WeeklyShare
    {
        public int OperationID { get; set; }
        public int CustomerID { get; set; }
        public decimal Portion { get; set; }
        public int Frequency { get; set; }

        public WeeklyShare(int operationID, int customerID, decimal portion, int frequency)
        {
            OperationID = operationID;
            CustomerID = customerID;
            Portion = portion;
            Frequency = frequency;
        }
        public WeeklyShare()
        {
            OperationID = 0;
            CustomerID = 0;
            Portion = 1.0m;
            Frequency = 1;
        }
    }
    public class WeeklyShareViewModel : WeeklyShare
    {
        public WeeklyShareViewModel() : base()
        {

        }
        public WeeklyShareViewModel(Operation operation, int userId) : base()
        {
            this.Operation = operation;
            this.OperationID = operation.OperationID;
            this.CustomerID = userId;
        }
        public Operation Operation { get; set; }
    }
}

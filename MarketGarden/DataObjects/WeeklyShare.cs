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
    }
}

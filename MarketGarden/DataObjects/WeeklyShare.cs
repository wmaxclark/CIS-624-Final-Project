using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class WeeklyShare
    {
        int OperationID { get; set; }
        int CustomerID { get; set; }
        decimal Portion { get; set; }
        int Frequency { get; set; }

        public WeeklyShare(int operationID, int customerID, decimal portion, int frequency)
        {
            OperationID = operationID;
            CustomerID = customerID;
            Portion = portion;
            Frequency = frequency;
        }
    }
}

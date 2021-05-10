using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public Operation()
        {
            this.OperationID = 0;
            this.UserID_Operator = 0;
            this.OperationName = "";
            this.AddressState = "";
            this.MaxShares = 0;
            this.Active = true;
        }

        public override string ToString()
        {
            return OperationName;
        }
    }
    public class OperationViewModel : Operation
    {
        public OperationViewModel(int operationID, User operatorUser, string operationName, string addressState, int? maxShares, bool active, List<Product> products, List<Order> orders, List<WeeklyShare> weeklyShares) : base(operationID, operatorUser.UserID, operationName, addressState, maxShares, active)
        {
            this.Operator = operatorUser;
            this.Products = products;
            this.Orders = orders;
            this.WeeklyShares = weeklyShares;
        }

        public OperationViewModel() : base()
        {

        }
        public User Operator { get; set; }
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
        public List<WeeklyShare> WeeklyShares { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class SubscribeOperationViewModel : Operation
    {
        public SubscribeOperationViewModel() : base()
        {
            Selection = false;
        }
        public SubscribeOperationViewModel(OperationViewModel model)
        {
            Selection = false;
            this.OperationID = model.OperationID;
            this.OperationName = model.OperationName;
            this.MaxShares = model.MaxShares;
            this.Active = model.Active;
            this.AddressState = model.AddressState;
            this.UserID_Operator = model.UserID_Operator;
            this.Products = model.Products;
        }
        [Required]
        public bool Selection { get; set; }

        public List<Product> Products { get; set; }
    }
}

using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class OrderLine
    {
        int OrderID { get; set; }
        int OrderLineID { get; set; }
        public int ProductID { get; set; }
        public decimal PriceCharged { get; set; }

        public OrderLine(int orderID, int productID, int orderLineID, decimal priceCharged)
        {
            OrderID = orderID;
            ProductID = productID;
            OrderLineID = orderLineID;
            PriceCharged = priceCharged;
        }

        public override string ToString()
        {

            return PriceCharged.ToString();
        }
    }
}
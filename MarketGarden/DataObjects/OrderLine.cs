using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class OrderLine
    {
        int OrderID { get; set; }
        int OrderLineID { get; set; }
        int ProductID { get; set; }
        decimal PriceCharged { get; set; }

        public OrderLine(int orderID, int productID, int orderLineID, decimal priceCharged)
        {
            OrderID = orderID;
            ProductID = productID;
            OrderLineID = orderLineID;
            PriceCharged = priceCharged;
        }
    }
}
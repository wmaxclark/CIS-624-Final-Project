using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class CartViewModel
    {
        public List<Product> Products { get; set; }

        public CartViewModel()
        {
            Products = new List<Product>();
        }
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Product
    {
        public int ProductID { get; private set; }
        public int OperationID { get; private set; }
        public string ProductName { get; private set; }
        public string ProductDescription { get; private set; }
        public string Unit { get; private set; }
        public decimal InputCost { get; private set; }
        public decimal UnitPrice { get; private set; }
        public DateTime GerminationDate { get; private set; }
        public int DaysAfterGerminationToPlant { get; private set; }
        public int DaysAfterGerminationToTransplant { get; private set; }
        public int DaysAfterGerminationToHarvest { get; private set; }

        public Product(int productID, int operationID, string productName, 
            string productDescription, string unit, decimal inputCost, 
            decimal unitPrice, DateTime germinationDate, 
            int daysAfterGerminationToPlant, 
            int daysAfterGerminationToTransplant, 
            int daysAfterGerminationToHarvest)
        {
            this.ProductID = productID;
            this.OperationID = operationID;
            this.ProductName = productName;
            this.ProductDescription = productDescription;
            this.Unit = unit;
            this.InputCost = inputCost;
            this.UnitPrice = unitPrice;
            this.GerminationDate = germinationDate;
            this.DaysAfterGerminationToPlant = daysAfterGerminationToPlant;
            this.DaysAfterGerminationToTransplant = daysAfterGerminationToTransplant;
            this.DaysAfterGerminationToHarvest = daysAfterGerminationToHarvest;
        }
    }
}
}

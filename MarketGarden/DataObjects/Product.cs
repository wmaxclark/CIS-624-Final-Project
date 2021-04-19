using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects.AttiributeValidation;

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
        public DateTime PlantDate { get; private set; }
        public DateTime TransplantDate { get; private set; }
        public DateTime HarvestDate { get; private set; }

        public Product(int productID, int operationID, string productName, 
            string productDescription, string unit, decimal inputCost, 
            decimal unitPrice, DateTime germinationDate,
            DateTime daysAfterGerminationToPlant,
            DateTime daysAfterGerminationToTransplant,
            DateTime daysAfterGerminationToHarvest)
        {
            this.ProductID = productID;
            this.OperationID = operationID;
            this.ProductName = productName;
            this.ProductDescription = productDescription;
            this.Unit = unit;
            this.InputCost = inputCost;
            this.UnitPrice = unitPrice;
            this.GerminationDate = germinationDate;
            this.PlantDate = daysAfterGerminationToPlant;
            this.TransplantDate = daysAfterGerminationToTransplant;
            this.HarvestDate = daysAfterGerminationToHarvest;
        }

    }
    public class CreateProductViewModel
    {
        [Required]
        public int OperationID { get; private set; }
        [Required]
        [Display(Name = "Name")]
        
        public string ProductName { get; private set; }
        [Required]
        [Display(Name = "Description")]
        public string ProductDescription { get; private set; }
        [Required]
        [Display(Name = "Unit for Distribution")]
        public string Unit { get; private set; }
        [Required]
        [Display(Name = "Average Input Cost per Unit")]
        public decimal InputCost { get; private set; }
        [Required]
        [Display(Name = "MSRP per Unit")]
        public decimal UnitPrice { get; private set; }
        [Required]
        [Display(Name = "Germination Date")]
        public DateTime GerminationDate { get; private set; }
        [Required]
        [Display(Name = "Plant Date")]
        public DateTime PlantDate { get; private set; }
        [Required]
        [Display(Name = "Transplant Date")]
        public DateTime TransplantDate { get; private set; }
        [Required]
        [Display(Name = "Harvest Date")]
        public DateTime HarvestDate { get; private set; }
    }
}


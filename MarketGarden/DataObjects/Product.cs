using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Product
    {
        public int ProductID { get; set; }
        public int OperationID { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }
        [Display(Name = "Distribution Unit")]
        public string Unit { get; set; }
        [Display(Name = "Input Cost")]
        public decimal InputCost { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime GerminationDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PlantDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TransplantDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HarvestDate { get; set; }

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

        public Product()
        {
            this.ProductID = 0;
            this.OperationID = 0;
            this.ProductName = "";
            this.ProductDescription = "";
            this.Unit = "";
            this.InputCost = 0M;
            this.UnitPrice = 0M;
            this.GerminationDate = DateTime.Today;
            this.PlantDate = DateTime.Today;
            this.TransplantDate = DateTime.Today;
            this.HarvestDate = DateTime.Today;
        }

    }
    public class CreateProductViewModel
    {
        public int OperationID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        [Required]
        [Display(Name = "Unit for Distribution")]
        public string Unit { get; set; }
        [Required]
        [Display(Name = "Average Input Cost per Unit")]
        public decimal InputCost { get; set; }
        [Required]
        [Display(Name = "MSRP per Unit")]
        public decimal UnitPrice { get; set; }
        [Required]
        [Display(Name = "Germination Date")]
        public DateTime GerminationDate { get; set; }
        [Required]
        [Display(Name = "Plant Date")]
        public DateTime PlantDate { get; set; }
        [Required]
        [Display(Name = "Transplant Date")]
        public DateTime TransplantDate { get; set; }
        [Required]
        [Display(Name = "Harvest Date")]
        public DateTime HarvestDate { get; set; }
    }
    public class EditProductViewModel
    {
        public EditProductViewModel(Product product)
        {
            this.ProductID = product.ProductID;
            this.OperationID = product.OperationID;
            this.ProductName = product.ProductName;
            this.ProductDescription = product.ProductDescription;
            this.Unit = product.Unit;
            this.InputCost = product.InputCost;
            this.UnitPrice = product.UnitPrice;
            this.GerminationDate = product.GerminationDate;
            this.PlantDate = product.PlantDate;
            this.TransplantDate = product.TransplantDate;
            this.HarvestDate = product.HarvestDate;
        }
        public EditProductViewModel()
        {
            this.ProductID = 0;
            this.OperationID = 0;
            this.ProductName = "";
            this.ProductDescription = "";
            this.Unit = "";
            this.InputCost = 0M;
            this.UnitPrice = 0M;
            this.GerminationDate = DateTime.Today;
            this.PlantDate = DateTime.Today;
            this.TransplantDate = DateTime.Today;
            this.HarvestDate = DateTime.Today;
        }

        [Required]
        public int OperationID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        [Required]
        [Display(Name = "Unit for Distribution")]
        public string Unit { get; set; }
        [Required]
        [Display(Name = "Average Input Cost per Unit")]
        public decimal InputCost { get; set; }
        [Required]
        [Display(Name = "MSRP per Unit")]
        public decimal UnitPrice { get; set; }
        [Required]
        [Display(Name = "Germination Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime GerminationDate { get; set; }
        [Required]
        [Display(Name = "Plant Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PlantDate { get; set; }
        [Required]
        [Display(Name = "Transplant Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TransplantDate { get; set; }
        [Required]
        [Display(Name = "Harvest Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HarvestDate { get; set; }
    }
    public class CloneProductViewModel
    {
        public CloneProductViewModel(Product product)
        {
            this.ProductID = product.ProductID;
            this.OperationID = product.OperationID;
            this.ProductName = product.ProductName;
            this.ProductDescription = product.ProductDescription;
            this.Unit = product.Unit;
            this.InputCost = product.InputCost;
            this.UnitPrice = product.UnitPrice;
            this.GerminationDate = product.GerminationDate;
        }
        public CloneProductViewModel()
        {
            this.ProductID = 0;
            this.OperationID = 0;
            this.ProductName = "";
            this.ProductDescription = "";
            this.Unit = "";
            this.InputCost = 0M;
            this.UnitPrice = 0M;
            this.GerminationDate = DateTime.Today;
        }

        [Required]
        public int OperationID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        [Required]
        [Display(Name = "Unit for Distribution")]
        public string Unit { get; set; }
        [Required]
        [Display(Name = "Average Input Cost per Unit")]
        public decimal InputCost { get; set; }
        [Required]
        [Display(Name = "MSRP per Unit")]
        public decimal UnitPrice { get; set; }
        [Required]
        [Display(Name = "Germination Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime GerminationDate { get; set; }
    }
    public class DeleteProductViewModel
    {
        public DeleteProductViewModel(Product product)
        {
            this.ProductID = product.ProductID;
            this.ProductName = product.ProductName;
            this.DeleteSelection = false;
        }
        public DeleteProductViewModel()
        {
            this.ProductID = 0;
            this.ProductName = "";
            this.DeleteSelection = false;
        }
        [Required]
        public int ProductID { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Name")]
        public bool DeleteSelection { get; set; }
    }
}

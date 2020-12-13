using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for frmProductAddEditDetail.xaml
    /// </summary>
    public partial class frmProductAddEditDetail : Window
    {
        private Product _selectedProduct;
        private bool _addProduct = false;
        private IOperationManager _operationManager = new OperationManager();
        private User user;
        private OperationVM operation;

        public frmProductAddEditDetail()
        {
            _addProduct = true;
            InitializeComponent();
        }

        public frmProductAddEditDetail(Product selectedProduct) // A product has been selected from the product list
        {
            this._selectedProduct = selectedProduct;
            txtProductName.Text = _selectedProduct.ProductName;
            txtProductDescription.Text = _selectedProduct.ProductDescription;
            txtUnit.Text = _selectedProduct.Unit;
            txtInputCost.Text = _selectedProduct.InputCost.ToString();
            txtUnitPrice.Text = _selectedProduct.UnitPrice.ToString();
            dptGerminationDate.SelectedDate = _selectedProduct.GerminationDate;
            txtDaysAfterToPlant.Text = _selectedProduct.DaysAfterGerminationToPlant.ToString();
            txtDaysAfterToHarvest.Text = _selectedProduct.DaysAfterGerminationToHarvest.ToString();
            txtDaysAfterToTransPlant.Text = _selectedProduct.DaysAfterGerminationToTransplant.ToString();


        }

        public frmProductAddEditDetail(User user, OperationVM operation, bool addProduct) // A new product is being created
        {
            this.user = user;
            this.operation = operation;
            this._addProduct = addProduct;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void btnSubmit_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSubmitButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

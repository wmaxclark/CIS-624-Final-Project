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
        private IOperationManager _operationManager;
        private OperationViewModel _operation;
        private Product _selectedProduct;
        private User _user;
        private bool _addProduct = false;
        private bool _isClone = false;

        public frmProductAddEditDetail()
        {
            _addProduct = true;
            InitializeComponent();
        }

        public frmProductAddEditDetail(User user, OperationViewModel operation, Product selectedProduct) // A product has been selected from the product list
        {
            this._user = user;
            this._operation = operation;
            this._selectedProduct = selectedProduct;
            _operationManager = new OperationManager();
            InitializeComponent();
        }

        public frmProductAddEditDetail(User user, OperationViewModel operation) // A new product is being created
        {
            this._user = user;
            this._operation = operation;
            _operationManager = new OperationManager();
            this._addProduct = true;
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_addProduct)
            {
                txtProductName.Text = _selectedProduct.ProductName;
                txtProductDescription.Text = _selectedProduct.ProductDescription;
                txtUnit.Text = _selectedProduct.Unit;
                txtInputCost.Text = _selectedProduct.InputCost.ToString();
                txtUnitPrice.Text = _selectedProduct.UnitPrice.ToString();
                dptGerminationDate.SelectedDate = _selectedProduct.GerminationDate;
                dptPlantDate.SelectedDate = _selectedProduct.PlantDate;
                dptTransplantDate.SelectedDate = _selectedProduct.TransplantDate;
                dptHarvestDate.SelectedDate = _selectedProduct.HarvestDate;

                btnSubmit.Content = "Save";
                
            }
            else
            {
                btnClone.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
            }
        }
        private void btnSubmit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (isValidProduct())
            {
                if (_addProduct) // Product is being created
                {
                    try
                    {
                        _operationManager.AddProduct(_operation.OperationID,
                        txtProductName.Text,
                        txtProductDescription.Text,
                        txtUnit.Text,
                        decimal.Parse(txtInputCost.Text),
                        decimal.Parse(txtUnitPrice.Text),
                        (DateTime)dptGerminationDate.SelectedDate,
                        (DateTime)dptPlantDate.SelectedDate,
                        (DateTime)dptTransplantDate.SelectedDate,
                        (DateTime)dptHarvestDate.SelectedDate);
                        // Operation has completed
                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Product could not be created." + ex.InnerException.Message);
                    }
                }
                else if (_isClone)
                {
                    try
                    {
                        _operationManager.AddProduct(_operation.OperationID,
                        txtProductName.Text,
                        txtProductDescription.Text,
                        txtUnit.Text,
                        decimal.Parse(txtInputCost.Text),
                        decimal.Parse(txtUnitPrice.Text),
                        (DateTime)dptGerminationDate.SelectedDate,
                        (DateTime)dptPlantDate.SelectedDate,
                        (DateTime)dptTransplantDate.SelectedDate,
                        (DateTime)dptHarvestDate.SelectedDate);
                        // Operation has completed
                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Product could not be created." + ex.InnerException.Message);
                    }
                }
                else
                {
                    try
                    {
                        _operationManager.UpdateProduct(_operation.OperationID,
                        _selectedProduct,
                        txtProductName.Text,
                        txtProductDescription.Text,
                        txtUnit.Text,
                        decimal.Parse(txtInputCost.Text),
                        decimal.Parse(txtUnitPrice.Text),
                        (DateTime)dptGerminationDate.SelectedDate,
                        (DateTime)dptPlantDate.SelectedDate,
                        (DateTime)dptTransplantDate.SelectedDate,
                        (DateTime)dptHarvestDate.SelectedDate);
                        // Operation has completed
                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Product could not be updated." + ex.InnerException.Message);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _operationManager.DeleteProduct((Product)_selectedProduct);
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Product could not be deleted." + ex.InnerException.Message);
                }
            }
            
        }

        private void btnClone_Click(object sender, RoutedEventArgs e)
        {
            btnDelete.Visibility = Visibility.Hidden;
            _isClone = true;
            txtProductName.Focus();
            btnSubmit.Content = "Save Clone";
        }

        private void dptGerminationDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (_isClone)
            {
                dptPlantDate.IsEnabled = false;
                dptTransplantDate.IsEnabled = false;
                dptHarvestDate.IsEnabled = false;

                // Calculate the interval between the germination date of the original product
                TimeSpan timeSpan = _selectedProduct.PlantDate - _selectedProduct.GerminationDate;

                // Add the interval to the date
                dptPlantDate.SelectedDate = dptGerminationDate.SelectedDate.Value.Add(timeSpan);
                
                // Calculate the interval between the germination date of the original product
                timeSpan = _selectedProduct.TransplantDate - _selectedProduct.GerminationDate;

                // Add the interval to the date
                dptTransplantDate.SelectedDate = dptGerminationDate.SelectedDate.Value.Add(timeSpan);
                
                // Calculate the interval between the germination date of the original product
                timeSpan = _selectedProduct.HarvestDate - _selectedProduct.GerminationDate;
                
                // Add the interval to the date
                dptHarvestDate.SelectedDate = dptGerminationDate.SelectedDate.Value.Add(timeSpan);
            }
        }
        private bool isValidProduct()
        {
            bool result = true;
            if (txtProductName.Text.Length > 64)
            {
                MessageBox.Show("Product Name too long.");
                txtProductName.Clear();
                txtProductName.Focus();
                result = false;
            }
            if (txtProductName.Text == "" || txtProductName.Text == null)
            {
                MessageBox.Show("Must have a product name");
                txtProductName.Focus();
                result = false;
            }
            if (txtProductDescription.Text.Length > 1024)
            {
                MessageBox.Show("Product Description too long.");
                txtProductDescription.Clear();
                txtProductDescription.Focus();
                result = false;
            }
            if (txtProductDescription.Text == "" || txtProductDescription.Text == null)
            {
                MessageBox.Show("Must have a product description.");
                txtProductDescription.Focus();
                result = false;
            }
            if (txtUnit.Text.Length > 64)
            {
                MessageBox.Show("Unit name too long.");
                txtUnit.Clear();
                txtUnit.Focus();
                result = false;
            }
            if (txtUnit.Text == "" || txtUnit.Text == null)
            {
                MessageBox.Show("Must have a unit of distribution.");
                txtUnit.Focus();
                result = false;
            }
            if (txtInputCost.Text == "" || txtInputCost.Text == null)
            {
                MessageBox.Show("Must have an input cost.");
                txtInputCost.Focus();
                result = false;
            }
            if (txtUnitPrice.Text == "" || txtUnitPrice.Text == null)
            {
                MessageBox.Show("Must have a unit price.");
                txtUnitPrice.Focus();
                result = false;
            }
            if (dptGerminationDate.SelectedDate == null)
            {
                MessageBox.Show("Must have a germination date.");
                dptGerminationDate.Focus();
                result = false;
            }
            if (dptPlantDate.SelectedDate == null)
            {
                MessageBox.Show("Must have a plant date.");
                dptPlantDate.Focus();
                result = false;
            }
            if (dptTransplantDate.SelectedDate == null)
            {
                MessageBox.Show("Must have a transplant date.");
                dptTransplantDate.Focus();
                result = false;
            }
            if (dptHarvestDate.SelectedDate == null)
            {
                MessageBox.Show("Must have a harvest date.");
                dptHarvestDate.Focus();
                result = false;
            }
            return result;
        }
    }
}

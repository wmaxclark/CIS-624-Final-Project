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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LogicLayer;
using DataObjects;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IUserManager _userManager = new UserManager();
        private IOperationManager _operationManager = new OperationManager();
        private User _user = null;
        private OperationViewModel _operation = null;
        private BindingList<Operation> _operationList;
        private const string newUserPassword = "newuser";
        private BindingList<Order> _orderList;
        /**
         * Cart must be a binding list to fire CollectionChanged events
         * Solution from https://stackoverflow.com/questions/4588359/implementing-collectionchanged
         * User: https://stackoverflow.com/users/114994/jay
         */
        private BindingList<Product> _productCart;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            resetWindow();
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == "Email")
            {
                txtEmail.Clear();
            }
        }

        private void pwdPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            lblPassword.Visibility = Visibility.Hidden;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btnLogin.Content == "Login")
            {
                try
                {
                    // Attempt to authenticate the user account matching the entered information
                    _user = _userManager.AuthenticateUser(txtEmail.Text, pwdPassword.Password);

                    // Check if the user account is new
                    if (pwdPassword.Password == newUserPassword)
                    {
                        // Instantiate form to update user account
                        var updatePassword = new frmCreateUpdateAccount(_userManager, _user, true, false, newUserPassword);

                        // 
                        if (!updatePassword.ShowDialog() == true)
                        {
                            _user = null;
                            resetWindow();
                            MessageBox.Show("You must change your password on first login.",
                                "Password Change Required", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                            return;
                        }
                    }
                    mnuMain.IsEnabled = true;
                    btnLogin.Content = "Logout";
                    pwdPassword.Password = "";
                    txtEmail.Text = "Email";
                    pwdPassword.Visibility = Visibility.Hidden;
                    lblPassword.Visibility = Visibility.Hidden;
                    txtEmail.Visibility = Visibility.Hidden;
                    mnuUpdateProfile.Visibility = Visibility.Visible;
                    if (_user.Roles.Count == 0)
                    {
                        MessageBox.Show("You have not chosen a role, please update your account.");
                    }
                    showUserTabs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
            }
            else
            {
                _user = null;
                resetWindow();
            }
        }

        private void resetWindow()
        {
            hideAllTabs();
            // Switch to an empty tab to resolve an issue with 
            // child elements of hidden tabitems still displaying
            tabMain.SelectedItem = tabItemEmpty;

            // Update the interface to the default state
            mnuMain.IsEnabled = true;
            mnuUpdateProfile.Visibility = Visibility.Collapsed;
            pwdPassword.Password = "";
            btnLogin.Content = "Login";
            txtEmail.Visibility = Visibility.Visible;
            txtEmail.Text = "Email";
            pwdPassword.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            

        }

        private void hideAllTabs()
        {
            // Iterate through tabs in tabmain and hide
            foreach (TabItem t in tabMain.Items)
            {
                t.Visibility = Visibility.Hidden;
            }
        }
        private void showUserTabs()
        {
            // Iterate through the users roles
            foreach (var r in _user.Roles)
            {
                // Determine the role, three currently available
                switch (r)
                {
                    case "Farmer":
                        tabItemOperationManagement.Visibility = Visibility.Visible;
                        tabItemOperationManagement.IsSelected = true;
                        tabItemOperationManagement.Focus();
                        break;
                    case "Customer":
                        tabItemCustomer.Visibility = Visibility.Visible;
                        tabItemCustomer.IsSelected = true;
                        break;
                }
            }
        }

        private void tabItemOperationManagement_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                _operation = _operationManager.GetOperationVMByOperator(_user);
                if (dgProductsList.ItemsSource == null)
                {
                    refreshProductsList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }


        private void mnuUpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Instantiate form to update user account
                var updatePassword = new frmCreateUpdateAccount(_userManager, _user, false, true, newUserPassword);

                if (!updatePassword.ShowDialog() == true)
                {

                    MessageBox.Show("Account not updated.");
                    return;
                }
                else
                {
                    _user = null;
                    resetWindow();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void mnuCreateProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Instantiate form to update user account
                var accountCreated = new frmCreateUpdateAccount(_userManager, false, false, newUserPassword);

                if (!accountCreated.ShowDialog() == true)
                {
                    _user = null;
                    resetWindow();
                    MessageBox.Show("Account not created.");
                    return;
                }
                else
                {
                    _user = null;
                    resetWindow();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void refreshProductsList()
        {
            dgProductsList.Visibility = Visibility.Visible;
            try
            {
                _operation.Products = _operationManager.RefreshProductList(_operation);
                dgProductsList.ItemsSource = _operation.Products;

                // Remove the header for the unique ID, not meaningful to user
                dgProductsList.Columns.Remove(dgProductsList.Columns[0]);
                dgProductsList.Columns.Remove(dgProductsList.Columns[0]);

                dgProductsList.Columns[0].Header = "Product Name";
                dgProductsList.Columns[1].Header = "Product Description";
                dgProductsList.Columns[2].Header = "Unit";
                dgProductsList.Columns[3].Header = "Input Cost";
                dgProductsList.Columns[4].Header = "Unit Price";
                dgProductsList.Columns[5].Header = "Germination Date";
                dgProductsList.Columns[6].Header = "Plant Date";
                dgProductsList.Columns[7].Header = "Transplant Date";
                dgProductsList.Columns[8].Header = "Harvest Date";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Product list could not be refreshed" + ex.InnerException.Message);
            }
            
        }
        private void dgProductsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedProduct = (Product)dgProductsList.SelectedItem;
            if (selectedProduct == null)
            {
                MessageBox.Show("You need to select a product in order to edit.",
                    "Edit Operation Not Available", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                var detailWindow = new frmProductAddEditDetail(_user, _operation, selectedProduct);
                if (detailWindow.ShowDialog() == true)
                {
                    refreshProductsList();
                }
            }
        }
        private void addProduct_Click(object sender, RoutedEventArgs e)
        {
            var detailWindow = new frmProductAddEditDetail(_user, _operation);
            if (detailWindow.ShowDialog() == true)
            {
                refreshProductsList();
            }
        }

        private void refreshWeeklySharesList()
        {
            try
            {
                _operation.WeeklyShares = _operationManager.RefreshWeeklyShares(_operation);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Weekly share list could not be refreshed" + ex.InnerException.Message);
            }
        }

        private void tabWeeklyShare_GotFocus(object sender, RoutedEventArgs e)
        {
            refreshWeeklySharesList();
            //_operation.WeeklyShares = _operationManager.RefreshWeeklyShares(_operation);
            var totalPortion = 0;
            var totalProfit = 0.0m;
            foreach (WeeklyShare share in _operation.WeeklyShares)
            {
                totalPortion++;
                foreach (var product in _operation.Products)
                {
                    totalProfit += product.UnitPrice - product.InputCost;
                }
            }
            lblShareCounter.Content = totalPortion + " subscribers";
            lblTotal.Content = "$" + totalProfit.ToString() + " weekly share profit";
        }

        private void tabDirectSale_GotFocus(object sender, RoutedEventArgs e)
        {
            refreshOrderList();
            var totalOrders = 0;
            var totalProfit = 0.0m;
            foreach (Order order in _operation.Orders)
            {
                totalOrders++;
                foreach (var line in order.Lines)
                {
                    foreach (var product in _operation.Products)
                    {
                        if (line.ProductID == product.ProductID)
                        {
                            totalProfit += line.PriceCharged - product.InputCost;
                        }
                    }
                     
                }
            }
            lblOrderCounter.Content = totalOrders + " orders";
            lblOrderTotal.Content = "$" + totalProfit.ToString() + " order profit";
        }

        private void refreshOrderList()
        {
            try
            {
                _operation.Orders = _operationManager.RefreshOrderList(_operation);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Order list could not be refreshed" + ex.InnerException.Message);
            }
        }

        private void cmbOperations_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbOperations.SelectedItem != null)
            {

                try
                {
                    var id = (Operation)cmbOperations.SelectedItem;
                    dgOrderProductList.ItemsSource = _operationManager.GetProductsByOperation(id.OperationID);

                    // Remove the header for the unique ID, not meaningful to user
                    dgOrderProductList.Columns.Remove(dgOrderProductList.Columns[0]);
                    dgOrderProductList.Columns.Remove(dgOrderProductList.Columns[0]);
                    dgOrderProductList.Columns.Remove(dgOrderProductList.Columns[3]);
                    dgOrderProductList.Columns.Remove(dgOrderProductList.Columns[4]);
                    dgOrderProductList.Columns.Remove(dgOrderProductList.Columns[4]);
                    dgOrderProductList.Columns.Remove(dgOrderProductList.Columns[4]);


                    dgOrderProductList.Columns[0].Header = "Product Name";
                    dgOrderProductList.Columns[1].Header = "Product Description";
                    dgOrderProductList.Columns[2].Header = "Unit";
                    dgOrderProductList.Columns[3].Header = "Unit Price";

                    btnCreateWeeklyShare.Visibility = Visibility.Visible;

                    bool isSubscribed = _operationManager.GetWeeklyShareByUser(_user, id.OperationID);
                    if (isSubscribed)
                    {
                        btnCreateWeeklyShare.IsEnabled = false;
                        btnCreateWeeklyShare.Content = "Subscribed";
                    }
                    else
                    {
                        btnCreateWeeklyShare.IsEnabled = true;
                        btnCreateWeeklyShare.Content = "Subscribe to CSA";
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Product list could not be loaded." + ex.InnerException.Message);
                }
            }
        }

        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomerCart.Items.Count != 0)
            {
                try
                {
                    var id = (Operation)cmbOperations.SelectedItem;
                    _operationManager.CreateOrder(_user, id.OperationID, DateTime.Now, _productCart);
                    _orderList = _operationManager.GetOrderListByUser(_user);
                    dgCustomerOrderList.ItemsSource = _orderList;
                    MessageBox.Show("Order Made!");
                    _productCart.Clear();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Order could not be created." + ex.InnerException.Message);
                }
            }
        }

        private void dgOrderProductList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedProduct = (Product)dgOrderProductList.SelectedItem;
            if (selectedProduct == null)
            {
                MessageBox.Show("You need to select a product in order to add to cart.");
            }
            else
            {
                _productCart.Add(selectedProduct);
                dgCustomerCart.ItemsSource = _productCart;
                updateTotalPrice();
            }
        }

        private void tabItemCustomer_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void cmbOperations_Initialized(object sender, EventArgs e)
        {
            
        }

        private void tabItemCustomer_Initialized(object sender, EventArgs e)
        {
            tabItemOperationManagement.Visibility = Visibility.Collapsed;
            btnCreateWeeklyShare.Visibility = Visibility.Hidden;

        }

        private void dgCustomerCart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            var selectedProduct = (Product)dgCustomerCart.SelectedItem;
            if (selectedProduct == null)
            {
                MessageBox.Show("You need to select a product in order to add to cart.");
            }
            else
            {
                
                _productCart.Remove(selectedProduct);
                dgCustomerCart.ItemsSource = _productCart;
                updateTotalPrice();

            }
        }

        private void updateTotalPrice()
        {
            if (_productCart.Count != 0)
            {
                var totalPrice = 0.0m;
                foreach (var item in _productCart)
                {
                    totalPrice += item.UnitPrice;
                }
                tbkTotalPrice.Text = "Total Price: " + totalPrice;
            }
            else
            {
                tbkTotalPrice.Text = "Total Price: ";
            }
            
        }

        private void dgCustomerCart_Initialized(object sender, EventArgs e)
        {
            _productCart = new BindingList<Product>();
        }

        private void dgCustomerOrderList_Initialized(object sender, EventArgs e)
        {
            _orderList = new BindingList<Order>();
            
        }

        private void dgCustomerOrderList_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                _orderList = _operationManager.GetOrderListByUser(_user);
                dgCustomerOrderList.ItemsSource = _orderList;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void dgOrderList_Initialized(object sender, EventArgs e)
        {
            try
            {
                //_orderList = _operationManager.GetOrderListByUser(_user);
                //dgCustomerOrderList.ItemsSource = _orderList;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }

        private void btnCreateWeeklyShare_Click(object sender, RoutedEventArgs e)
        {
            if (cmbOperations.SelectedItem != null)
            {

                try
                {
                    var id = (Operation)cmbOperations.SelectedItem;

                    if (_operationManager.CreateWeeklyShare(_user, id.OperationID, 1.0m, 1))
                    {
                        MessageBox.Show("Weekly Share Created!");
                        btnCreateWeeklyShare.IsEnabled = false;
                        btnCreateWeeklyShare.Content = "Subscribed";

                    }
                    
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Product list could not be loaded." + ex.InnerException.Message);
                }
            }
        }

        private void cmbOperations_Drop(object sender, DragEventArgs e)
        {
            
        }

        private void cmbOperations_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void cmbOperations_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _operationList = _operationManager.GetAllOperations();
                cmbOperations.ItemsSource = _operationList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }
    }
}

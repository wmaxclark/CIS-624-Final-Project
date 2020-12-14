﻿using System;
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
        private OperationVM _operation = null;
        private List<Operation> _operationList;
        private const string newUserPassword = "newuser";
        private List<Order> _orderList = null;
        private List<Product> _productCart;

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
            if (_operation.MaxShares.HasValue)
            {
                _operation.WeeklyShares = _operationManager.RefreshWeeklyShares(_operation);
                var totalPortion = 0.0m;
                var totalProfit = 0.0m;
                foreach (WeeklyShare share in _operation.WeeklyShares)
                {
                    totalPortion += share.Portion;
                    foreach (var product in _operation.Products)
                    {
                        totalProfit += product.UnitPrice - product.InputCost;
                    }
                }
                txtShareCounter.Text = totalPortion.ToString() + " out of: " +  _operation.MaxShares.ToString() + " ordered";
                txtTotal.Text = "$" + totalProfit.ToString() + " weekly share profit";
            }
        }

        private void tabDirectSale_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                _operation.Orders = _operationManager.RefreshOrderList(_operation);
                dgOrderList.ItemsSource = _operation.Orders;
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
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Product list could not be loaded." + ex.InnerException.Message);
                }
            }
        }

        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {

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
                refreshCustomerCart(selectedProduct);
                
            }
        }

        private void refreshCustomerCart(Product selectedProduct)
        {
            dgCustomerCart.Visibility = Visibility.Visible;
            if (_productCart == null)
            {
                _productCart = new List<Product>();
                _productCart.Add(selectedProduct);
                dgCustomerCart.ItemsSource = _productCart;
                // Remove the header for the unique ID, not meaningful to user
                dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[0]);
                dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[0]);
                dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[3]);
                dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[4]);
                dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[4]);
                dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[4]);
                dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[4]);


                dgCustomerCart.Columns[0].Header = "Product Name";
                dgCustomerCart.Columns[1].Header = "Product Description";
                dgCustomerCart.Columns[2].Header = "Unit";
                dgCustomerCart.Columns[3].Header = "Unit Price";
            }
            else
            {
                _productCart.Add(selectedProduct);
                dgCustomerCart.ItemsSource = _productCart;
                //// Remove the header for the unique ID, not meaningful to user
                //dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[0]);
                //dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[0]);
                //dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[3]);
                //dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[4]);
                //dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[4]);
                //dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[4]);
                //dgCustomerCart.Columns.Remove(dgCustomerCart.Columns[4]);


                //dgCustomerCart.Columns[0].Header = "Product Name";
                //dgCustomerCart.Columns[1].Header = "Product Description";
                //dgCustomerCart.Columns[2].Header = "Unit";
                //dgCustomerCart.Columns[3].Header = "Unit Price";
            }
        }

        private void tabItemCustomer_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void cmbOperations_Initialized(object sender, EventArgs e)
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

        private void tabItemCustomer_Initialized(object sender, EventArgs e)
        {
            tabItemOperationManagement.Visibility = Visibility.Collapsed;
        }

    }
}

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
        private const string newUserPassword = "newuser";

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

            dgProductsList.ItemsSource = null;
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
                    case "Helper":
                        tabItemHelper.Visibility = Visibility.Visible;
                        tabItemHelper.IsSelected = true;
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

        private void refreshProductsList()
        {
            dgProductsList.Visibility = Visibility.Visible;
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
            dgProductsList.Columns[6].Header = "Days after Germination to plant";
            dgProductsList.Columns[7].Header = "Days after Germination to transplant";
            dgProductsList.Columns[8].Header = "Days after Germination to harvest";
        }
        private void dgProductsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedProduct = (Product)dgProductsList.SelectedItem;
            if (selectedProduct == null)
            {
                MessageBox.Show("You need to select a product in order to edit.", 
                    "Edit Operation Not Available", MessageBoxButton.OK, 
                    MessageBoxImage.Information);

                var detailWindow = new frmProductAddEditDetail(selectedProduct);
                if (detailWindow.ShowDialog() == true)
                {
                    refreshProductsList();
                }
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
        private void addProduct_Click(object sender, RoutedEventArgs e)
        {

        }
        private void tabItemCSA_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void tabItemRestaraunt_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void tabMarketStall_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void tabDirectSale_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void tabItemOperationManagement_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        
    }
}

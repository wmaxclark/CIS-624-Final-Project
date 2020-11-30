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
        private User _user = null;
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
                        var updatePassword = new frmCreateUpdateAccount(_userManager, _user, true, newUserPassword);

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

            }
            catch (Exception)
            {

                throw;
            }
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
    }
}

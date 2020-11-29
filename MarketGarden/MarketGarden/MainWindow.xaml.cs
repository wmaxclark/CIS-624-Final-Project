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
            resetLogin();
        }

        private void tabItemOperationManagement_GotFocus(object sender, RoutedEventArgs e)
        {

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

        private void resetLogin()
        {
            txtEmail.Text = "Email";
            lblPassword.Visibility = Visibility.Visible;
            pwdPassword.Password = "";
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
                        var updatePassword = new frmCreateUpdateAccount(_userManager, _user, true);

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
            mnuMain.IsEnabled = true;
            pwdPassword.Password = "";
            btnLogin.Content = "Login";
            txtEmail.Visibility = Visibility.Visible;
            txtEmail.Text = "Email";
            pwdPassword.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            txtEmail.Focus();
        }

        private void hideAllTabs()
        {
            foreach (TabItem t in tabMain.Items)
            {
                t.Visibility = Visibility.Collapsed;

            }
        }
        private void showUserTabs()
        {
            foreach (var r in _user.Roles)
            {
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


    }
}

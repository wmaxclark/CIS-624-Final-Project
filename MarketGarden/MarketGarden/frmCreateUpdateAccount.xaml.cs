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
    /// Interaction logic for frmLogin.xaml
    /// </summary>
    public partial class frmCreateUpdateAccount : Window
    {
        private IUserManager _userManager;
        private User _user;
        private bool _isNewUserAccount;
        private string newUserPassword;

        public frmCreateUpdateAccount()
        {
            InitializeComponent();
        }

        public frmCreateUpdateAccount(IUserManager userManager, User user, bool isNewUserAccount, string newUserPassword)
        {

            this._userManager = userManager;
            this._user = user;
            this._isNewUserAccount = isNewUserAccount;
            this.newUserPassword = newUserPassword;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isNewUserAccount)
            {
                changePasswordHandler();
            }
            else
            {
                txtEmail.IsEnabled = true;
                txtFirstName.Focus();
            }
        }

        private void changePasswordHandler()
        {
            tbkMessage.Text = "Update your password";
            txtFirstName.Text = _user.FirstName;
            txtFirstName.IsEnabled = false;
            txtLastName.Text = _user.LastName;
            txtLastName.IsEnabled = false;
            txtEmail.Text = _user.Email;
            txtEmail.IsEnabled = false;
            pwdPassword.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string oldPassword = pwdPassword.Password;
            string newPassword = pwdNewPassword.Password;
            string retypePassword = pwdRetypePassword.Password;

            if (!email.isValidEmail() || email != _user.Email)
            {
                MessageBox.Show("Invalid Email");
                txtEmail.Clear();
                txtEmail.Focus();
                return;
            }
            if (!newPassword.isValidPassword() || newPassword == newUserPassword)
            {
                MessageBox.Show("Invalid Password");
                pwdNewPassword.Clear();
                pwdNewPassword.Focus();
                return;
            }
            if (retypePassword != newPassword)
            {
                MessageBox.Show("Passwords must match");
                pwdRetypePassword.Clear();
                pwdRetypePassword.Focus();
                return;
            }
            try
            {
                if (_userManager.UpdatePassword(_user.Email, pwdPassword.Password, pwdNewPassword.Password))
                {
                    // If all checks have succeeded
                    MessageBox.Show("Password Updated.");
                    this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
            }
        }
    }
}

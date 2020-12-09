﻿using DataObjects;
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
        private bool _isExistingAccount;
        private string newUserPassword;
        private List<string> roleList;

        public frmCreateUpdateAccount()
        {
            _userManager = new UserManager();
            InitializeComponent();
        }

        public frmCreateUpdateAccount(IUserManager userManager, User user, bool isNewUserAccount, bool isExistingAccount, string newUserPassword)
        {

            this._userManager = userManager;
            this._user = user;
            this._isNewUserAccount = isNewUserAccount;
            this._isExistingAccount = isExistingAccount;
            this.newUserPassword = newUserPassword;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            roleList = _userManager.getAllRoles();
            cmbUserRoles.ItemsSource = roleList;
            if (_isNewUserAccount)
            {
                changePasswordHandler();
            }
            else if (_isExistingAccount)
            {
                updateAccountHandler();
            }
            else
            {
                createAccountHandler();
            }
        }

        private void createAccountHandler()
        {
            tbkMessage.Text = "Create your account";
            lblNewPassword.Visibility = Visibility.Collapsed;
            pwdNewPassword.Visibility = Visibility.Collapsed;
        }

        private void updateAccountHandler()
        {
            tbkMessage.Text = "Update your account";
            lblPassword.Visibility = Visibility.Collapsed;
            pwdPassword.Visibility = Visibility.Collapsed;

            txtFirstName.Text = _user.FirstName;
            txtLastName.Text = _user.LastName;
            txtEmail.Text = _user.Email;
            txtEmail.IsEnabled = false;
            cmbUserRoles.SelectedItem = _user.Roles[0];
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
            cmbUserRoles.IsEnabled = false;
            cmbUserRoles.SelectedItem = _user.Roles[0];
            pwdPassword.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string oldPassword = pwdPassword.Password;
            string newPassword = pwdNewPassword.Password;
            string retypePassword = pwdRetypePassword.Password;
            if (_isNewUserAccount)
            {
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
            else if (_isExistingAccount)
            {
                if (!email.isValidEmail() || email != txtEmail.Text)
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
                    if (_userManager.UpdateUserRole(email, cmbUserRoles.SelectedItem.ToString()) 
                        && _userManager.UpdatePassword(_user.Email, pwdPassword.Password, pwdNewPassword.Password))
                    {
                        // If all checks have succeeded
                        MessageBox.Show("Account Updated.");
                        this.DialogResult = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
            }
            else // An account is being created.
            {
                if (!email.isValidEmail() || email != txtEmail.Text)
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
                    var id = _userManager.CreateUserAccount(email, txtFirstName.Text, txtLastName.Text, pwdPassword.Password);
                    if (id != 0)
                    {
                        _userManager.CreateUserRole(id, cmbUserRoles.SelectedItem.ToString());
                        // If all checks have succeeded
                        MessageBox.Show("Account Created.");
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
}

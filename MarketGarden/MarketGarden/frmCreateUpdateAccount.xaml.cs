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
        private IOperationManager _operationManager;
        private User _user;
        private bool _isNewUserAccount;
        private bool _isExistingAccount;
        private string _newUserPassword;
        private List<string> _roleList;
        private List<string> _states;
        private List<Operation> _operations;

        public frmCreateUpdateAccount()
        {
            _userManager = new UserManager();
            _operationManager = new OperationManager();
            InitializeComponent();
        }

        public frmCreateUpdateAccount(IUserManager userManager, User user, bool isNewUserAccount, bool isExistingAccount, string newUserPassword)
        {

            this._userManager = userManager;
            _operationManager = new OperationManager();
            this._user = user;
            this._isNewUserAccount = isNewUserAccount;
            this._isExistingAccount = isExistingAccount;
            this._newUserPassword = newUserPassword;
            InitializeComponent();
        }

        public frmCreateUpdateAccount(IUserManager userManager, bool isNewUserAccount, bool isExistingAccount, string newUserPassword)
        {
            this._userManager = userManager;
            _operationManager = new OperationManager();
            this._newUserPassword = newUserPassword;
            this._isNewUserAccount = isNewUserAccount;
            this._isExistingAccount = isExistingAccount;
            this._newUserPassword = newUserPassword;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _roleList = _userManager.getAllRoles();
            cmbUserRoles.ItemsSource = _roleList;
            _states = _operationManager.GetAllStates();
            cmbStates.ItemsSource = _states;
            _operations = _operationManager.GetAllOperations();
            cmbOperations.ItemsSource = _operations;
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
            lblOperation.Visibility = Visibility.Collapsed;
            lblOperationName.Visibility = Visibility.Collapsed;
            txtOperationName.Visibility = Visibility.Collapsed;

            cmbOperations.Visibility = Visibility.Collapsed;
            txtFirstName.Focus();
        }

        private void updateAccountHandler()
        {
            tbkMessage.Text = "Update your account";
            lblStates.Visibility = Visibility.Collapsed;
            cmbStates.Visibility = Visibility.Collapsed;
            lblOperation.Visibility = Visibility.Collapsed;
            cmbOperations.Visibility = Visibility.Collapsed;
            lblOperationName.Visibility = Visibility.Collapsed;
            txtOperationName.Visibility = Visibility.Collapsed;
            lblRole.Visibility = Visibility.Collapsed;
            cmbUserRoles.Visibility = Visibility.Collapsed;

            txtFirstName.Text = _user.FirstName;
            txtLastName.Text = _user.LastName;
            txtEmail.Text = _user.Email;
            txtEmail.IsEnabled = false;
            cmbUserRoles.SelectedItem = _user.Roles[0];
        }

        private void changePasswordHandler() // An existing user updating their account
        {
            tbkMessage.Text = "Update your password";
            txtFirstName.Text = _user.FirstName;
            txtFirstName.IsEnabled = false;
            txtLastName.Text = _user.LastName;
            txtLastName.IsEnabled = false;
            txtEmail.Text = _user.Email;
            txtEmail.IsEnabled = false;
            lblStates.Visibility = Visibility.Collapsed;
            cmbStates.Visibility = Visibility.Collapsed;
            lblOperation.Visibility = Visibility.Collapsed;
            cmbOperations.Visibility = Visibility.Collapsed;
            lblOperationName.Visibility = Visibility.Collapsed;
            txtOperationName.Visibility = Visibility.Collapsed;

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
                if (!newPassword.isValidPassword() || newPassword == _newUserPassword)
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
            else if (_isExistingAccount) // An existing user updating their account
            {
                if (!email.isValidEmail() || email != txtEmail.Text)
                {
                    MessageBox.Show("Invalid Email");
                    txtEmail.Clear();
                    txtEmail.Focus();
                    return;
                }
                if (!newPassword.isValidPassword() || newPassword == _newUserPassword)
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
                        MessageBox.Show("Account Updated, please log in to continue.");
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
                if (!oldPassword.isValidPassword() || oldPassword == _newUserPassword)
                {
                    MessageBox.Show("Invalid Password");
                    pwdNewPassword.Clear();
                    pwdNewPassword.Focus();
                    return;
                }
                if (retypePassword != oldPassword)
                {
                    MessageBox.Show("Passwords must match");
                    pwdRetypePassword.Clear();
                    pwdRetypePassword.Focus();
                    return;
                }
                try
                {
                    // Create the user account and capture the id for later operations
                    var id = _userManager.CreateUserAccount(email, txtFirstName.Text, txtLastName.Text, pwdPassword.Password);

                    _user = _userManager.AuthenticateUser(email, pwdPassword.Password);
                    // If the account was created, create the role selected
                    if (id != 0)
                    {
                        if (cmbUserRoles.SelectedItem.ToString() == _roleList[0]) // User is a customer
                        {
                            _userManager.CreateUserRole(_user.UserID, cmbUserRoles.SelectedItem.ToString());
                        }
                        else if (cmbUserRoles.SelectedItem.ToString() == _roleList[1]) // User is a Farmer
                        {
                            try
                            {
                                // Create the farm
                                _operationManager.CreateOperation(_user.UserID, cmbStates.SelectedItem.ToString(), txtOperationName.Text);
                                try
                                {
                                    _userManager.CreateUserRole(_user.UserID, _operationManager.GetOperationByOperator(_user), _roleList[1]);
                                }
                                catch (Exception ex)
                                {

                                    throw ex;
                                }
                            }
                            catch (Exception ex)
                            {
                                // TODO make this presentable to user
                                MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                            }
                            
                        }
                        else // User is a helper
                        {
                            
                            _userManager.CreateUserRole(id, (Operation)cmbOperations.SelectedItem, cmbUserRoles.SelectedItem.ToString());
                        }
                        // If all checks have succeeded
                        MessageBox.Show("Account created, please log in to continue.");
                        this.DialogResult = true;
                    }
                }
                catch (Exception ex)
                {
                    // TODO make this presentable to the user
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
            }
        }

        private void cmbUserRoles_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbUserRoles.SelectedItem != null)
            {
                // Handle interface for customers
                if (cmbUserRoles.SelectedItem.ToString() == _roleList[0])
                {
                    lblOperationName.Visibility = Visibility.Hidden;
                    txtOperationName.Visibility = Visibility.Hidden;
                    lblOperation.Visibility = Visibility.Hidden;
                    cmbOperations.Visibility = Visibility.Hidden;
                }
                // Handle interface for farmers
                if (cmbUserRoles.SelectedItem.ToString() == _roleList[1])
                {
                    lblOperationName.Visibility = Visibility.Visible;
                    txtOperationName.Visibility = Visibility.Visible;
                }
                // Handle interface for helpers
                if (cmbUserRoles.SelectedItem.ToString() == _roleList[2])
                {
                    lblOperationName.Visibility = Visibility.Hidden;
                    txtOperationName.Visibility = Visibility.Hidden;
                    lblOperation.Visibility = Visibility.Visible;
                    cmbOperations.Visibility = Visibility.Visible;
                    try
                    {
                        //TODO get the items source cmbOperations.ItemsSource
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                    }
                }
            }
        }

        private void cmbUserRoles_DropDownOpened(object sender, EventArgs e)
        {
            _roleList = _userManager.getAllRoles();
            cmbUserRoles.ItemsSource = _roleList;
        }
    }
}

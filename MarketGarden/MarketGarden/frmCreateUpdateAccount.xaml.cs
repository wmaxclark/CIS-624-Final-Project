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

        public frmCreateUpdateAccount()
        {
            InitializeComponent();
        }

        public frmCreateUpdateAccount(IUserManager userManager, User user, bool isNewUserAccount)
        {

            this._userManager = userManager;
            this._user = user;
            this._isNewUserAccount = isNewUserAccount;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

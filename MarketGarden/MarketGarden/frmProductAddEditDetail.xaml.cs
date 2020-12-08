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
        private Product _selectedProduct;
        private bool _addProduct = false;
        private IOperationManager _operationManager = new OperationManager();

        public frmProductAddEditDetail()
        {
            _addProduct = true;
            InitializeComponent();
        }

        public frmProductAddEditDetail(Product selectedProduct)
        {
            this._selectedProduct = selectedProduct;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

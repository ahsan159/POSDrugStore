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
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace POSStore
{
    /// <summary>
    /// Interaction logic for salesWindow.xaml
    /// </summary>
    public partial class salesWindow : Window
    {
        public salesWindow()
        {
            InitializeComponent();            
            sqlWrapper wrapper = sqlWrapper.getInstance();
            DataTable dt = wrapper.executeQuery("invoiceLedger");
            MessageBox.Show(dt.Rows.Count.ToString());
            invoiceTable.ItemsSource = dt.DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

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
            DataTable dt = wrapper.getTable("invoiceLedger");
            startDate.SelectedDate = DateTime.Now;
            endDate.SelectedDate = DateTime.Now;
            //MessageBox.Show(dt.Rows.Count.ToString());
            invoiceTable.ItemsSource = dt.DefaultView;
            totalSale.Text = calculateTotal(dt).ToString();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void calenderDateChanged(object sender, RoutedEventArgs e)
        {
            sqlWrapper wrapper = sqlWrapper.getInstance();
            string dtS = startDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            string dtE = endDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            string query = @"SELECT * FROM invoiceLedger where CheckoutDate BETWEEN '" +
                    dtS +
                    "' and '" +
                    dtE +
                    "';";
            DataTable dt =  wrapper.executeBasicQuery(query);
            invoiceTable.ItemsSource = dt.DefaultView;
            totalSale.Text = calculateTotal(dt).ToString();
        }
        private double calculateTotal(DataTable table)
        {
            double total = 0;
            foreach(DataRow dr in table.Rows)
            {
                total += double.Parse(dr["Total"].ToString());
            }
            return total;
        }
    }
}

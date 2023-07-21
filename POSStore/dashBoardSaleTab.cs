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
    /// for some unknown reason beyond my understanding 
    /// every time I have to reload data in datatable and 
    /// update my datagrid. I have to clear my rows and
    /// reload a binded datatable from buffer datatable.
    /// However, correct way is reset datatable and load
    /// from buffer.
    /// Some times I directly read from SQL and datagrid 
    /// shows updated datatable. and some times I have
    /// to follow afore mentioned method.
    /// </summary>
    public partial class dashBoard : Window
    {
        public int selectedInvoiceIndex { get; set; } = 0;
        public string totalBilled { get; set; }
        public string cashPresented { get; set; }
        public string discountGiven { get; set; }
        public string balanceReturned { get; set; }


        private void initializeSaleTableTab()
        {
            endDatesaleTab.SelectedDate = DateTime.Now;
            startDatesaleTab.SelectedDate = DateTime.Now;
            //calenderDateChanged(this, new RoutedEventArgs());
        }
        private void calenderDateChanged(object sender, RoutedEventArgs e)
        {
            sqlWrapper wrapper = sqlWrapper.getInstance();
            string dtS = startDatesaleTab.SelectedDate.Value.ToString("yyyy-MM-dd");
            string dtE = endDatesaleTab.SelectedDate.Value.ToString("yyyy-MM-dd");
            string query = @"SELECT * FROM invoiceLedger where CheckoutDate BETWEEN '" +
                    dtS +
                    "' and '" +
                    dtE +
                    "';";
            DataTable dt = wrapper.executeBasicQuery(query);
            invoiceSaleTableDT.Rows.Clear();
            invoiceSaleTableDT.Load(dt.CreateDataReader());
            //invoiceTablesaleTab.ItemsSource = invoiceSaleTableDT.DefaultView;
            totalSalesaleTab.Text = calculateTotal(invoiceSaleTableDT).ToString();

        }
        private double calculateTotal(DataTable table)
        {
            double total = 0;
            foreach (DataRow dr in table.Rows)
            {
                total += double.Parse(dr["Total"].ToString());
            }
            return total;
        }
        public void invoiceTable_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs evt)
        {
            try
            {
                if (selectedInvoiceIndex > 0)
                {
                    string str = invoiceSaleTableDT.Rows[selectedInvoiceIndex]["DBName"].ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        DataTable dtBuf = dWrap.executeQuery(str);
                        //invoiceSaleTable.ItemsSource = displaySaleTableDT.DefaultView;
                        displaySaleTableDT.Rows.Clear();
                        displaySaleTableDT.Load(dtBuf.CreateDataReader());
                        //MessageBox.Show(totalBilled + Environment.NewLine +
                        //    cashPresented);
                    }
                    totalBilledL.Content = invoiceSaleTableDT.Rows[selectedInvoiceIndex]["Total"].ToString(); ;
                    cashPresentedL.Content = invoiceSaleTableDT.Rows[selectedInvoiceIndex]["Payment"].ToString(); ;
                    discountGivenL.Content = invoiceSaleTableDT.Rows[selectedInvoiceIndex]["Discount"].ToString(); ;
                    balanceReturnedL.Content = invoiceSaleTableDT.Rows[selectedInvoiceIndex]["Balance"].ToString(); ;
                }
            }
            catch (Exception) { }

        }
    }
}

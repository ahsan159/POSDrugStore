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
    public partial class dashBoard : Window
    {
        public int selectedInvoiceIndex { get; set; } = 0;
        public string totalBilled { get; set; } = "?????";
        public string cashPresented { get; set; }
        public string discountGiven { get; set; }
        public string balanceReturned { get; set; }
        public DataTable displaySaleTableDT { get; set; } = new DataTable();
        public DataTable invoiceSaleTableDT { get; set; } = new DataTable();

        private void initializeSaleTableTab()
        {
            endDatesaleTab.SelectedDate = DateTime.Now;
            startDatesaleTab.SelectedDate = DateTime.Now;
            calenderDateChanged(this, new RoutedEventArgs());
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
            invoiceSaleTableDT = wrapper.executeBasicQuery(query);
            invoiceTablesaleTab.ItemsSource = invoiceSaleTableDT.DefaultView;
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
            if (selectedInvoiceIndex > 0)
            {
                string str = invoiceSaleTableDT.Rows[selectedInvoiceIndex]["DBName"].ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    displaySaleTableDT = dWrap.executeQuery(str);
                    invoiceSaleTable.ItemsSource = displaySaleTableDT.DefaultView;
                    //MessageBox.Show(totalBilled + Environment.NewLine +
                    //    cashPresented);
                }
                totalBilledL.Content = invoiceSaleTableDT.Rows[selectedInvoiceIndex]["Total"].ToString(); ;
                cashPresentedL.Content = invoiceSaleTableDT.Rows[selectedInvoiceIndex]["Payment"].ToString(); ;
                discountGivenL.Content = invoiceSaleTableDT.Rows[selectedInvoiceIndex]["Discount"].ToString(); ;
                balanceReturnedL.Content = invoiceSaleTableDT.Rows[selectedInvoiceIndex]["Balance"].ToString(); ;
            }
            
        }
    }
}

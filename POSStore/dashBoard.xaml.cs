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
    /// Interaction logic for dashBoard.xaml
    /// </summary>
    public partial class dashBoard : Window
    {
        sqlWrapper wrap = sqlWrapper.getInstance();
        sqlWrapper dWrap = sqlWrapper.getInstance();
        public DataTable displaySaleTableDT { get; set; } = new DataTable();
        public DataTable invoiceSaleTableDT { get; set; } = new DataTable();
        public dashBoard()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            dashBoardOpen(this, new RoutedEventArgs());
            this.DataContext = this;

            // for stock Tab
            stockCollection = dWrap.getTable("stockTable");
            //DataRow dr = stockCollection.NewRow();
            //stockCollection.Rows.Add(dr);
            stockTable.ItemsSource = stockCollection.DefaultView;
            initializeStockTableTab();

            // for sale Tab
            initializeSaleTableTab();
            invoiceSaleTableDT = dWrap.executeBasicQuery("SELECT * FROM invoiceLedger");
            //endDatesaleTab.SelectedDate = DateTime.Now;
            //startDatesaleTab.SelectedDate = DateTime.Now;
            //calenderDateChanged(this, new RoutedEventArgs());

            //for drug List Tab
            initializeDrugListTab();

            // for new sale
            initializeNewSale();
        }        
        private void closeWindow(object sender, RoutedEventArgs evt)
        {
            this.Close();
        }
        private void stockWindowOpen(object sender, RoutedEventArgs evt)
        {
            stockWindow sWin = new stockWindow();
            sWin.ShowDialog();
            this.Show();
        }
        private void openDrugList(object sender, RoutedEventArgs evt)
        {
            //this.Hide();
            //MainWindow mw = new MainWindow();
            //mw.ShowDialog();
            //this.Show();
            producttab.Focus();
            updateDashText();            
        }
        private void openCustomerList(object sender, RoutedEventArgs evt)
        {
            updateDashText();            
        }
        private void openSaleList(object sender, RoutedEventArgs evt)
        {
            saletab.Focus();
            updateDashText();
            
        }
        private void openStockList(object sender, RoutedEventArgs evt)
        {
            stockstab.Focus();
            updateDashText();
            
        }
        private void dashBoardOpen(object sender, RoutedEventArgs evt)
        {
            updateDashText();            
        }
        private void updateDashText()
        {
            int drugCount = wrap.itemCount("mainLedger");
            pBtn.Content = "Products" + Environment.NewLine + "(" + drugCount.ToString() + ")";
            int stockCount = wrap.itemCount("stockTable");
            stockBtn.Content = "Stocks " + Environment.NewLine + "(" + stockCount.ToString() + ")";
            int saleCount = wrap.itemCount("mainLedger");
            saleBtn.Content = "Sales " + Environment.NewLine + "(" + saleCount.ToString() + ")";
            int customerCount = 0;
            cBtn.Content = "Customer " + Environment.NewLine + "(" + customerCount.ToString() + ")";
        }


    }
}

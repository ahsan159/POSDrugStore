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
using System.ComponentModel;
using System.IO.Pipes;
using registryData;

namespace POSStore
{
    /// <summary>
    /// Interaction logic for dashBoard.xaml
    /// </summary>
    public partial class dashBoard : Window,INotifyPropertyChanged
    {
        sqlWrapper wrap = sqlWrapper.getInstance();
        sqlWrapper dWrap = sqlWrapper.getInstance();
        registryDataClass regClass = new();
        public DataTable? productListDT { get; set; } = new DataTable("productList");
        public List<string?> _drugListComboItems { get; set; } = new();
        public List<string?> drugListComboItems
        {
            get
            {
                return _drugListComboItems;
            }
            set
            {
                _drugListComboItems = value;
                NotifyPropertyChanged("drugListComboItems");
            }
        }

        private void NotifyPropertyChanged(string v)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(v));
            }
        }

        public List<int>? drugListComboID { get; set; } = new List<int>();
        public List<Single>? drugListComboCost { get; set; } = new List<Single>();
        public DataTable displaySaleTableDT { get; set; } = new DataTable();
        public DataTable invoiceSaleTableDT { get; set; } = new DataTable();
        public string? User { get; set; } = string.Empty;
        public dashBoard()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            dashBoardOpen(this, new RoutedEventArgs());
            this.DataContext = this;
            User = regClass.getLoginName();
            string userlevel = regClass.getLevel();
            if (regClass.getActivityStatus() != "LoggedIn")
            {
                this.Close();
            }
            if(userlevel!="admin")
            {
                ConfigTab.Visibility = Visibility.Hidden;
            }
            else
            {
                ConfigTab.Visibility = Visibility.Visible;
            }
            // for stock Tab            
            //DataRow dr = stockCollection.NewRow();
            //stockCollection.Rows.Add(dr);
            //stockTable.ItemsSource = stockCollection.DefaultView;
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

            // for cutomer tab initialization
            updateCustomerTable();

            initializeConfigTab();

            SetGraphs();
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
            int saleCount = wrap.itemCount("invoiceLedger");
            saleBtn.Content = "Sales " + Environment.NewLine + "(" + saleCount.ToString() + ")";
            int customerCount = 0;
            cBtn.Content = "Customer " + Environment.NewLine + "(" + customerCount.ToString() + ")";
        }
        private void updateTables()
        {
            // this function is to update all the tables across various tabs
        }

        private void ProductTab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("Product Tab Selected");
            refreshProducts();
        }
        private void SaleTab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            refreshSales();
        }
        private void StockTab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            refreshStocks();
        }
        private void CustomerTab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            refreshCustomers();
        }
        private void NewSaleTab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            refreshNewSale();
        }

        private void SetGraphs()
        {
            List<string> stack = new List<String>();
            List<string> dateLabel = new List<string>();
            List<string> graphLabel = new List<string>();
            List<string> moneyLabel = new List<string>();
            for (int  i = 0; i< 7; i++)
            {
                stack.Add("G" + (i + 1).ToString());
                dateLabel.Add("GD" + (i + 1).ToString());
                graphLabel.Add("GG" + (i + 1).ToString());
                moneyLabel.Add("GA" + (i + 1).ToString());

            }    
            foreach(string s in stack)
            {
                StackPanel c = this.FindName(s) as StackPanel;
                c.Visibility = Visibility.Hidden;
            }
            DataTable dt = dWrap.getTable("invoiceLedger");
            int index = 0;
            foreach(DataRow dr in dt.Rows)
            {
                if (index>6) { break; }
                //MessageBox.Show(dateLabel.Count().ToString() + Environment.NewLine + index);
                Label l1 = this.FindName(dateLabel[index]) as Label;
                l1.Content = dr["DBName"].ToString();
                Label l2 = this.FindName(moneyLabel[index]) as Label;
                l2.Content = dr["Total"].ToString();
                StackPanel s = this.FindName(stack[index]) as StackPanel;
                s.Visibility = Visibility.Visible;
                //MessageBox.Show(dr["Total"].ToString());
                int length1 = (int)double.Parse(dr["Total"].ToString())/10;
                Label l3 = this.FindName(graphLabel[index]) as Label;
                l3.Content = new string(' ', length1);
                index++;
            }
            
            
            //MessageBox.Show(GetDaySales(dt,"2023-09-30").ToString());


        }

        private int GetDaySales(DataTable dt, string day)
        {
            int sum = 0;
            var result = dt.AsEnumerable().Where(r => r.Field<DateTime>("CheckoutDate") == DateTime.Parse(day));
            foreach (DataRow d in result)
            {
                sum += int.Parse(d["Total"].ToString());
            }
            return sum;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBox.Show("exiting");
            regClass.setActivityStatus("LoggedOut");
        }
    }
}

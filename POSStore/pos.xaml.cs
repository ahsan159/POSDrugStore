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
    /// Interaction logic for pos.xaml
    /// </summary>

    public partial class pos : Window
    {
        public string connectionString = "Data Source=ENG-RNR-05;Initial Catalog = DSPOS; Integrated Security = True";
        //public string connectionString = "Data Source=AHSAN-PC\\SQLExpress;Initial Catalog=DSPOS;Integrated Security=True;Pooling=False";
        public string queryString = "SELECT * from mainLedger";        
        public string selectValueCombo;
        public SqlConnection connection;
        public SqlDataAdapter dataAdapter;
        public DataTable saleCollection = new DataTable("saleTable");
        public DataTable invoiceCollection = new DataTable();
        public List<string> dList = new List<string>();
        public List<string> idList = new List<string>();
        private sqlWrapper sWrap = sqlWrapper.getInstance();
        private string saleTableName = string.Empty;
        public pos()
        {
            InitializeComponent();
            DataContext = this;
            this.WindowState = WindowState.Maximized;

            string dateCode = DateTime.Now.ToString("yyyyMMdd");
            invoiceNo.Text = @"Invoice\" + dateCode + @"\" + sWrap.itemCount("invoiceLedger").ToString();
            saleTableName = invoiceNo.Text.Replace(@"\", "_");
            //MessageBox.Show(saleTableName);
            saleTableSQL();
            DataTable dTable = new DataTable();
            try
            {
                connection = new SqlConnection(connectionString);
                dataAdapter = new SqlDataAdapter(queryString, connectionString);
                dataAdapter.Fill(dTable);
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Connot Load Data from SQL database" +
                 Environment.NewLine + e.Message +
                 Environment.NewLine + e.Source,
                 "Error",
                 MessageBoxButton.OK,
                 MessageBoxImage.Error
                 );
            }

            //saleCollection.Rows.Add(dr);
            saleCollection.Columns.Add("Name", typeof(string));
            saleCollection.Columns.Add("Quantity", typeof(string));
            saleCollection.Columns.Add("Price", typeof(string));
            saleCollection.Columns.Add("Discount100", typeof(string));
            saleCollection.Columns.Add("Discount", typeof(string));
            saleCollection.Columns.Add("Total", typeof(string));
            saleCollection.Columns.Add("ID",typeof(string));
            saleCollection.Columns.Add("Stock", typeof(string));
            DataRow dr = saleCollection.NewRow();
            saleCollection.Rows.Add(dr);
            saleTable.ItemsSource = saleCollection.DefaultView;

            //DataGridComboBoxColumn dgc = saleTable.Columns[0] as DataGridComboBoxColumn;

            dList = getDrugList() ;
            (saleTable.Columns[0] as DataGridComboBoxColumn).ItemsSource = dList;
            saleTable.SelectedIndex = 0;
            populateInvoiceTable();
            //dgc.ItemsSource = getDrugList();


        }
        private void saleTableSQL()
        {
            /// this method create a new unique table for evert invoice
            /// generated same table name is saved in invoiceledger
            string query = @"CREATE TABLE " + saleTableName + 
                            @" (
                           [Sr] INT NOT NULL IDENTITY(1,1),
                           [Name] VARCHAR(50) NOT NULL,
                           [Quantity] INT NOT NULL, 
                           [Price] REAL NOT NULL, 
                           [Discount100]  REAL NULL,
                           [Discount] REAL NULL,
                           [Sale Tax] REAL NULL,
                           [Total] REAL NOT NULL,
                           [ID] INT NULL,
                           [Stock] INT NULL
                        )";
            sWrap.executeNonQuery(query);            
            //MessageBox.Show(sWrap.errorMessage);
        }
        public List<string> getDrugList()
        {
            string cString = "SELECT DISTINCT(name),id FROM mainLedger;";
            connection = new SqlConnection(connectionString);
            dataAdapter = new SqlDataAdapter(cString, connection);
            DataTable dTable = new DataTable();
            dataAdapter.Fill(dTable);
            connection.Close();
            List<string> list = dTable.AsEnumerable().Select(c => c.Field<string>("name")).ToList();
            idList = dTable.AsEnumerable().Select(c => c.Field<int>("id").ToString()).ToList();
            return list;
        }
        public DataTable getDatafromSQL(string cString)
        {
            // this function wil get data from SQL 
            // actually this will only get whole drug list from the mainLedger
            connection = new SqlConnection(connectionString);
            dataAdapter = new SqlDataAdapter(cString, connectionString);
            DataTable dTable = new DataTable();
            dataAdapter.Fill(dTable);
            connection.Close();
            return dTable;
        }
        private void saleTable_Selected(object sender, SelectedCellsChangedEventArgs e)
        {
            //int i = 0;
            ////List<DataGridCellInfo> cells = saleTable.SelectedCells.ToList();            
            ////ComboBox cellContent = cells[i].Column.GetCellContent(cells[i].Item) as ComboBox;            
            //
            /// step 1: add new row
            if (saleTable.Items.Count - 1 == saleTable.SelectedIndex)
            {
                DataRow dr = saleCollection.NewRow();
                saleCollection.Rows.Add(dr);
                saleTable.SelectedIndex = saleTable.Items.Count - 2;
            }
            /// step 2: update drug list because you can update drug list
            /// during the data entry
            dList = getDrugList();
            (saleTable.Columns[0] as DataGridComboBoxColumn).ItemsSource = dList;
            /// step 3: make calculation for sale
            try
            {
                solveTable();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + Environment.NewLine +
                    exp.Source + Environment.NewLine +
                    exp.StackTrace, "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            /// step 4: update side display
            try
            {
                drugName.Content = saleCollection.Rows[saleTable.SelectedIndex]["Name"];
                price.Content = saleCollection.Rows[saleTable.SelectedIndex]["Price"];
                stock.Content = saleCollection.Rows[saleTable.SelectedIndex]["Stock"];
            }
            catch (Exception) { }
            /// step 5: checkout calculations
            grandTotal();
        }
        private void getCell()
        {

        }

        private void itemName_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show(dList[int.Parse(selectValueCombo)]);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + Environment.NewLine +
                    exp.StackTrace + Environment.NewLine +
                    exp.Source,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This function is called when a drug is selected from the list
            ComboBox cb = sender as ComboBox;            
            List<DataGridCellInfo> cells = saleTable.SelectedCells.ToList();
            //TextBlock tb = cells[3].Column.GetCellContent(cells[3].Item) as TextBlock;
            string query = @"SELECT * FROM mainLedger WHERE id='" +
                idList[cb.SelectedIndex] + "';";
            DataTable dTable = getDatafromSQL(query);
            DataRow dr = dTable.Rows[0];
            saleCollection.Rows[saleTable.SelectedIndex]["Name"] = dr["name"];
            saleCollection.Rows[saleTable.SelectedIndex]["Quantity"] = ""; // because quantity is to be entered by user
            saleCollection.Rows[saleTable.SelectedIndex]["Price"] = dr["Cost"];
            saleCollection.Rows[saleTable.SelectedIndex]["Discount"] = "";
            saleCollection.Rows[saleTable.SelectedIndex]["Discount100"] = "";
            saleCollection.Rows[saleTable.SelectedIndex]["Total"] = "0";
            saleCollection.Rows[saleTable.SelectedIndex]["ID"] = dr["id"];
            saleCollection.Rows[saleTable.SelectedIndex]["Stock"] = dr["quantity"];

            drugName.Content = dr["name"];
            price.Content = dr["Cost"];
            stock.Content = dr["quantity"];
            solveTable();            
        }

        private void saleTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //trying to use this function but it is not working atleast for me
            try
            {
                //solveTable();
                //MessageBox.Show("Editing");
                TextBox tb = e.EditingElement.DataContext as TextBox;
                MessageBox.Show(tb.Text);
                solveTable();
            }
            catch (Exception exp)
            {
                foreach (DataRow dr in saleCollection.Rows)
                {
                    dr["Total"] = "Editing" + e.Column.Header;
                }
                //MessageBox.Show(exp.Message + Environment.NewLine +
                //        exp.Source + Environment.NewLine +
                //        exp.StackTrace, "Error",
                //        MessageBoxButton.OK,
                //        MessageBoxImage.Error);
            }
        }

        private void solveTable()
        {
            // This function will solve the table and calculate the discounted amount and total amount            
            double total = 0;
            double disc = 0;
            double price = 0;
            double quantity = 0;
            try
            {
                double totalBilled = 0;
                foreach (DataRow dr in saleCollection.Rows)
                {
                    total = 0;
                    disc = 0;
                    price = 0;
                    quantity = 0;
                    bool pBool = double.TryParse(dr["Price"].ToString(), out price);
                    if (!pBool)
                    { 
                        dr["Total"] = "0.00";                        
                    }                    

                    bool qBool = double.TryParse(dr["Quantity"].ToString(), out quantity);
                    if(!qBool)
                    {
                        dr["Total"] = price.ToString();                                                
                    }
                    else
                    {
                        total = price*quantity;                    
                        dr["Total"] = total.ToString();
                    }


                    if (!string.IsNullOrEmpty(dr["Discount100"].ToString()))
                    {
                        disc = total * double.Parse(dr["Discount100"].ToString()) / 100;
                        if (disc > 0)
                        {
                            dr["Discount"] = disc.ToString();
                        }
                        total = price * quantity;
                        total -= disc;
                        dr["Total"] = total.ToString();
                    }
                    else if (!string.IsNullOrEmpty(dr["Discount"].ToString()))
                    {
                        disc = double.Parse(dr["Discount"].ToString());
                        total = price * quantity;
                        total -= disc;
                        dr["Total"] = total.ToString();
                    }
                    else
                    {
                        dr["Total"] = total.ToString();
                    }
                    //total -= disc;                    
                    dr["Total"] = total.ToString();
                    totalBilled += total;
                    totalCost.Content = totalBilled.ToString();
                    grandTotal();
                }
                
            }
            catch (Exception exp)
            {
                //MessageBox.Show(exp.Message + Environment.NewLine +
                //    exp.Source + Environment.NewLine +
                //    exp.StackTrace, "Error",
                //    MessageBoxButton.OK,
                //    MessageBoxImage.Error);
            }
        }

        private void grandTotal()
        {
            // calculate the grand total value and checkout process
            try
            {
                double totalC = double.Parse(totalCost.Content.ToString());
                double paidT = 0;
                double discT = 0;
                try
                {
                    paidT = double.Parse(paidTotal.Text.ToString());
                }
                catch (Exception) { }
                try
                {
                    discT = double.Parse(discountTotal.Text.ToString());
                }
                catch (Exception) { }
                double balmc = paidT - totalC + discT;
                balanceTotal.Text = balmc.ToString();
            }
            catch (Exception exp)
            {
                //MessageBox.Show(exp.Message + Environment.NewLine +
                //    exp.Source + Environment.NewLine +
                //    exp.StackTrace, "Error",
                //    MessageBoxButton.OK,
                //    MessageBoxImage.Error);
            }
        }


        private void deleteDataRow(object sender, RoutedEventArgs e)
        {
            // delete data row
            try
            {
                saleCollection.Rows.RemoveAt(saleTable.SelectedIndex);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + Environment.NewLine +
                    exp.Source + Environment.NewLine +
                    exp.StackTrace, "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private void closePOS(object sender, RoutedEventArgs e)
        {
            try
            {

                connection.Close();
                saleCollection.WriteXml("backupdata.xml");
            }
            catch (Exception exp)
            {
                //MessageBox.Show(exp.Message + Environment.NewLine +
                //    exp.Source + Environment.NewLine +
                //    exp.StackTrace, "Error",
                //    MessageBoxButton.OK,
                //    MessageBoxImage.Error);
            }
            this.Close();
        }


        private void paidTotal_KeyPress(object sender, KeyEventArgs e)
        {              
            if (e.Key.Equals(Key.Enter))
            {
                checkOut(this, new RoutedEventArgs());
            }
        }

        private void paidTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            solveTable();
            grandTotal();
        }


        private void openMainLedger(object sender, RoutedEventArgs evt)
        {
            MainWindow win = new MainWindow();
            win.ShowDialog();
        }
        private void checkOut(object sender, RoutedEventArgs evt)
        {
            // get column names from datatable
            //List<string> str = sWrap.columnList("invoiceLedger");
            //str.RemoveRange(0, 1);
            //MessageBox.Show(string.Join(",", str),sWrap.commandStatus);
            updateStock();
            saveSale();
            saveInvoice();
            clearSaleTable();
        }
        
        private void updateStock()
        {
            foreach(DataRow dr in saleCollection.Rows)
            {
                if(!string.IsNullOrEmpty(dr["Name"].ToString()))
                {
                    string UpdateQuery = @"UPDATE mainLedger " +
                        @"SET quantity='" + (int.Parse(dr["Stock"].ToString()) - int.Parse(dr["Quantity"].ToString())).ToString() + "'" +
                        @" WHERE id='" + dr["ID"] + "';";
                    //MessageBox.Show(UpdateQuery);
                    sWrap.executeNonQuery(UpdateQuery);
                }
            }
        }

        private void saveSale()
        {
            /// this function will save all the data of drugs their
            /// price and quantity to the seperate saleTable
            List<string> dt = saleCollection.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList<string>();
            // insert data in sql table one by one            
            foreach (DataRow dr in saleCollection.Rows)
            {
                
                try
                {
                    if (!string.IsNullOrEmpty(dr["Name"].ToString().Trim()))
                    {
                        var ds = dr.ItemArray;
                        string query = @"INSERT INTO " + saleTableName + " (" + string.Join(",", dt) +
                            ") values ('" +
                            string.Join("','", ds) + "');";
                        sWrap.executeNonQuery(query);
                    
                    }                
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message + Environment.NewLine +
                        exp.Source + Environment.NewLine +
                        exp.StackTrace, "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void saveInvoice()
        {
            /// this function will save the invoice data 
            /// payment, discount and balance not limiting to 
            /// customer data to invoiceLedger
            string totalString = totalCost.Content.ToString();
            string paidString = paidTotal.Text;
            string balanceString = balanceTotal.Text;
            string discountString = discountTotal.Text;
            int i = sWrap.itemCount("invoiceLedger");
            List<string> str = sWrap.columnList("invoiceLedger");
            int count = saleCollection.Rows.Count;
            string dateCode = DateTime.Now.ToString("yyyyMMdd");
            string dateData = DateTime.Now.ToString("yyyy-MM-dd");
            string timeData = DateTime.Now.ToString("hh:mm:ss");

            // insert invoice in invoice ledger
            //List<string> dt = invoiceCollection.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList<string>();
            invoiceCollection.Rows[0]["Invoice"] = @"Invoice\" + dateCode + @"\" + i.ToString();
            invoiceCollection.Rows[0]["Total"] = totalString;
            invoiceCollection.Rows[0]["Payment"] = paidString;
            invoiceCollection.Rows[0]["Balance"] = balanceString;
            invoiceCollection.Rows[0]["Discount"] = discountString;
            invoiceCollection.Rows[0]["CheckoutDate"] = dateData;
            invoiceCollection.Rows[0]["CheckoutTime"] = timeData;
            invoiceCollection.Rows[0]["Customer"] = customerName.Text;
            invoiceCollection.Rows[0]["Contact"] = contactNo.Text;
            invoiceCollection.Rows[0]["DrugCount"] = count.ToString();
            invoiceCollection.Rows[0]["UserName"] = "";
            invoiceCollection.Rows[0]["PaymentType"] = "";
            invoiceCollection.Rows[0]["Sale_Tax"] = " ";
            invoiceCollection.Rows[0]["DBName"] = saleTableName;

            DataRow dr = invoiceCollection.Rows[0];
            string cString = @"INSERT INTO invoiceLedger(" + string.Join(",", str) +
                ") Values ('" +
                string.Join("','", dr.ItemArray) +
                "');";
            sWrap.executeNonQuery(cString);
        }

        private void clearSaleTable()
        {
            //string queryClear = @"DELETE FROM testTable;";
            //executeNonQuery(queryClear);
            saleCollection.Rows.Clear();                 
            totalCost.Content = "";
            paidTotal.Text = "";
            balanceTotal.Text = "";
            discountTotal.Text = "";
            string dateCode = DateTime.Now.ToString("yyyyMMdd");
            invoiceNo.Text = @"Invoice\" + dateCode + @"\" + sWrap.itemCount("invoiceLedger").ToString();
            saleTableName = invoiceNo.Text.Replace(@"\", "_");
            saleTableSQL();
            customerName.Text = "";
            contactNo.Text = "";
            stock.Content = "";
            price.Content = "";

        }

        private void populateInvoiceTable()
        {
            string cString = @"select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='invoiceLedger';";
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cString, connection);
            adapter.Fill(dt);
            connection.Close();
            foreach(DataRow drow in dt.Rows)
            {
                string itemName = drow.ItemArray[0].ToString() ;
                invoiceCollection.Columns.Add(itemName);
            }
            DataRow dr = invoiceCollection.NewRow();
            invoiceCollection.Rows.Add(dr);
        }

        private void saleList(object sender, RoutedEventArgs evt)
        {
            salesWindow sWin = new salesWindow();
            sWin.ShowDialog();
        }

        private void openStockWindow(object sender, RoutedEventArgs evt)
        {
            stockWindow stockW = new stockWindow();
            stockW.ShowDialog();
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            (sender as ComboBox).IsDropDownOpen = true;
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as ComboBox).IsDropDownOpen = false;
        }

        private void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as ComboBox).IsDropDownOpen = true;
        }
    }
}


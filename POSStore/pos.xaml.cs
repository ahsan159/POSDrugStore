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
        public DataTable GridCollection = new DataTable();
        public DataTable invoiceCollection = new DataTable();
        public List<string> dList = new List<string>();
        public List<string> idList = new List<string>();
        public pos()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
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
            //GridCollection.Rows.Add(dr);
            GridCollection.Columns.Add("Name", typeof(string));
            GridCollection.Columns.Add("Quantity", typeof(string));
            GridCollection.Columns.Add("Price", typeof(string));
            GridCollection.Columns.Add("Discount100", typeof(string));
            GridCollection.Columns.Add("Discount", typeof(string));
            GridCollection.Columns.Add("Total", typeof(string));
            DataRow dr = GridCollection.NewRow();
            GridCollection.Rows.Add(dr);
            saleTable.ItemsSource = GridCollection.DefaultView;
            DataContext = this;
            DataGridComboBoxColumn dgc = saleTable.Columns[0] as DataGridComboBoxColumn;
            dList = getDrugList();            
            (saleTable.Columns[0] as DataGridComboBoxColumn).ItemsSource = dList;
            saleTable.SelectedIndex = 0;
            populateInvoiceTable();
            //dgc.ItemsSource = getDrugList();


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
            // step 1: add new row
            if (saleTable.Items.Count - 1 == saleTable.SelectedIndex)
            {
                DataRow dr = GridCollection.NewRow();
                GridCollection.Rows.Add(dr);
                saleTable.SelectedIndex = saleTable.Items.Count - 2;
            }            
            // step 2: update drug list
            dList = getDrugList();
            (saleTable.Columns[0] as DataGridComboBoxColumn).ItemsSource = dList;
            // step 3: make calculation for sale
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
            // step 4: update side display
            try
            {
                drugName.Content = GridCollection.Rows[saleTable.SelectedIndex]["name"];
                price.Content = GridCollection.Rows[saleTable.SelectedIndex]["Cost"];
            }
            catch (Exception) { }
            // step 5: checkout calculations
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
            GridCollection.Rows[saleTable.SelectedIndex]["Name"] = dr["name"];
            GridCollection.Rows[saleTable.SelectedIndex]["Quantity"] = ""; // because quantity is to be entered by user
            GridCollection.Rows[saleTable.SelectedIndex]["Price"] = dr["Cost"];
            GridCollection.Rows[saleTable.SelectedIndex]["Discount"] = "";
            GridCollection.Rows[saleTable.SelectedIndex]["Discount100"] = "";
            GridCollection.Rows[saleTable.SelectedIndex]["Total"] = "0";
            drugName.Content = dr["name"];
            price.Content = dr["Cost"];
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
                foreach (DataRow dr in GridCollection.Rows)
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
                foreach (DataRow dr in GridCollection.Rows)
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
                GridCollection.Rows.RemoveAt(saleTable.SelectedIndex);
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
                GridCollection.WriteXml("backupdata.xml");
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
            List<string> dt = GridCollection.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList<string>();
            // insert data in sql table one by one            
            foreach(DataRow dr in GridCollection.Rows)
            {
                var ds = dr.ItemArray;
                string query = @"INSERT INTO testTable(" + string.Join(",", dt) +
                    ") values ('" +
                    string.Join("','", ds) + "');";
                //MessageBox.Show(query);
                try
                {
                    executeNonQuery(query);
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
            clearSaleTable();
        }

        private void clearSaleTable()
        {
            //string queryClear = @"DELETE FROM testTable;";
            //executeNonQuery(queryClear);
            string totalString = totalCost.Content.ToString();
            string paidString = paidTotal.Text;
            string balanceString = balanceTotal.Text;
            string discountString = discountTotal.Text;
            int i = getInvoiceCount()+1;
            //App.Current.Shutdown(0);
            int count = GridCollection.Rows.Count;
            string dateCode = DateTime.Now.ToString("yyyyMMdd");
            string dateData = DateTime.Now.ToString("yyyy-MM-dd");
            string timeData = DateTime.Now.ToString("hh:mm:ss");
            GridCollection.Rows.Clear();
            totalCost.Content = "";
            paidTotal.Text = "";
            balanceTotal.Text = "";
            discountTotal.Text = "";

            // insert invoice in invoice ledger
            List<string> dt = invoiceCollection.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList<string>();
            invoiceCollection.Rows[0]["Invoice"] = @"Invoice\\" + dateCode + "\\" + i.ToString();
            invoiceCollection.Rows[0]["Total"] = totalString;
            invoiceCollection.Rows[0]["Payment"] = paidString;
            invoiceCollection.Rows[0]["Balance"] = balanceString;
            invoiceCollection.Rows[0]["Discount"] = discountString;
            invoiceCollection.Rows[0]["CheckoutDate"] = dateData;
            invoiceCollection.Rows[0]["CheckoutTime"] = timeData;
            invoiceCollection.Rows[0]["Customer"] = customerName.Text;
            invoiceCollection.Rows[0]["Contact"] = contactNo.Text;
            invoiceCollection.Rows[0]["DrugCount"] = count.ToString();
            invoiceCollection.Rows[0]["UserName"] = "AFE";
            invoiceCollection.Rows[0]["PaymentType"] = "AGDE";
            invoiceCollection.Rows[0]["Sale_Tax"] = " ";

            DataRow dr = invoiceCollection.Rows[0];
            string cString = @"INSERT INTO invoiceLedger(" + string.Join(",", dt) +
                ") Values ('" +
                string.Join("','", dr.ItemArray) +
                "');";
            MessageBox.Show(cString);
            executeNonQuery(cString);

        }

        private void executeNonQuery(string cString)
        {
            // not only add data but also refresh the datagrid table
            SqlCommand cmd = new SqlCommand(cString, connection);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();            
        }

        private int getInvoiceCount()
        {
            DataTable dt = new DataTable();
            string cString = @"SELECT COUNT(*) FROM invoiceLedger;";
            SqlDataAdapter adapter = new SqlDataAdapter(cString, connection);
            adapter.Fill(dt);
            //MessageBox.Show( dt.Rows[0].ItemArray[0].ToString());
            int id;
            bool iParse = int.TryParse(dt.Rows[0].ItemArray[0].ToString(), out id);
            connection.Close();
            adapter.Dispose();
            if (iParse)
            {
                return id;
            }
            return -1;
        }

        private void populateInvoiceTable()
        {
            string cString = @"select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='invoiceLedger';";
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cString, connection);
            adapter.Fill(dt);
            foreach(DataRow drow in dt.Rows)
            {
                string itemName = drow.ItemArray[0].ToString() ;
                invoiceCollection.Columns.Add(itemName);
            }
            DataRow dr = invoiceCollection.NewRow();
            invoiceCollection.Rows.Add(dr);
        }

    }
}


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
        public List<string> dList = new List<string>();
        public List<string> idList = new List<string>();
        public pos()
        {
            InitializeComponent();

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
            //GridCollection.Rows[saleTable.SelectedIndex]["qty"] = dr["quantity"]; // because quantity is to be entered by user
            GridCollection.Rows[saleTable.SelectedIndex]["Price"] = dr["Cost"];
            GridCollection.Rows[saleTable.SelectedIndex]["Discount"] = "";
            GridCollection.Rows[saleTable.SelectedIndex]["Discount100"] = "";
            GridCollection.Rows[saleTable.SelectedIndex]["Total"] = "0";
            drugName.Content = dr["name"];
            price.Content = dr["Cost"];
            solveTable();            
        }

        private void solveTable()
        {
            // This function will solve the table and calculate the discounted amount and total amount
            try
            {
                double totalBilled = 0;
                foreach (DataRow dr in GridCollection.Rows)
                {                    
                    double total = double.Parse(dr["Quantity"].ToString()) * double.Parse(dr["Price"].ToString());
                    double disc = 0;
                    if (!string.IsNullOrEmpty(dr["Discount100"].ToString()))
                    {
                        disc = double.Parse(dr["Price"].ToString()) * double.Parse(dr["Discount100"].ToString()) / 100;
                        if (disc > 0)
                        {
                            dr["Discount"] = disc.ToString();
                        }
                    }
                    else if (!string.IsNullOrEmpty(dr["Discount"].ToString()))
                    {
                        disc = double.Parse(dr["Discount"].ToString());                        
                    }
                    total -= disc;                    
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

        private void saleTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // trying to use this function but it is not working atleast for me
            //testEdit.Text = "cellEditing";
            //try
            //{                                    
            //    solveTable();                
            //}
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message + Environment.NewLine +
            //        exp.Source + Environment.NewLine +
            //        exp.StackTrace, "Error",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //}
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
        private void paidTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            grandTotal();
        }
        private void openMainLedger(object sender, RoutedEventArgs evt)
        {
            MainWindow win = new MainWindow();
            win.ShowDialog();
        }
        private void checkOut(object sender, RoutedEventArgs evt)
        {
            List<string> dt = GridCollection.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList<string>();
            //DataRow dr = GridCollection.Rows[0];
            //var ds = dr.ItemArray;
            //MessageBox.Show(string.Join(",", dt)
            //    + Environment.NewLine +
            //    string.Join(",",ds));
            foreach(DataRow dr in GridCollection.Rows)
            {
                var ds = dr.ItemArray;
                string query = @"INSERT INTO testTable(" + string.Join(",", dt) +
                    ") values ('" +
                    string.Join("','", ds) + "');";
                MessageBox.Show(query);
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
        }

        private void executeNonQuery(string cString)
        {
            // not only add data but also refresh the datagrid table
            SqlCommand cmd = new SqlCommand(cString, connection);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();            
        }
    }
}


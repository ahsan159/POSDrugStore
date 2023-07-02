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

namespace POSStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        //public string connectionString = "Data Source=ENG-RNR-05;Initial Catalog = DSPOS; Integrated Security = True";
        public string? connectionString = "Data Source=AHSAN-PC\\SQLExpress;Initial Catalog=DSPOS;Integrated Security=True;Pooling=False";
        public string queryString = "SELECT * from mainLedger";
        public SqlConnection? connection;
        public SqlDataAdapter? dataAdapter;
        public MainWindow()
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
            drugLedger.DataContext = dTable.DefaultView;

        }

        private void drugLedger_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.Key == Key.Enter)
            {
                List<DataGridCellInfo> cells = drugLedger.SelectedCells.ToList();
                string[] newData = new string[5];
                for (int i = 0; i < 5; i++)
                {
                    var cellContent = cells[i].Column.GetCellContent(cells[i].Item);
                    TextBlock cellText = (TextBlock)cellContent;
                    newData[i] = cellText.Text;
                    //MessageBox.Show(newData[i] + Environment.NewLine + i.ToString());
                }

                if (string.IsNullOrEmpty(newData[0].ToString().Trim()))
                {
                    string cString = @"insert into mainLedger(name,manufacturer,Cost,quantity) values ('" +
                    newData[1].ToString() + "','" +
                    newData[2].ToString() + "','" +
                    newData[4].ToString() + "','" +
                    newData[3].ToString() + "');";

                    //MessageBox.Show(cString);
                    addDatatoTable(cString);
                }
                else
                {
                    //MessageBox.Show("Possible Duplicate Entry")
                }
            }
            */
        }
        private void addDatatoTable(string cString)
        {
            // not only add data but also refresh the datagrid table
            SqlCommand cmd = new SqlCommand(cString, connection);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            dataAdapter = new SqlDataAdapter(queryString, connectionString);
            DataTable dTable = new DataTable();
            dataAdapter.Fill(dTable);
            drugLedger.DataContext = null;
            drugLedger.DataContext = dTable.DefaultView;
            // connection.Close();
        }
        private bool validateEntry()
        {
            /// this function is to vertify that user
            /// is not selecting invalid entry
            /// only check if first column of table is filled
            bool valid = false;
            List<DataGridCellInfo> cells = drugLedger.SelectedCells.ToList();
            string idData = string.Empty;
            var cellContent = cells[0].Column.GetCellContent(cells[0].Item);
            TextBlock cellText = (TextBlock)cellContent;
            idData = cellText.Text.Trim();
            if (!string.IsNullOrEmpty(idData))
            {
                return true;
            }
            return valid;

        }
        private void viewEntry(object sender, RoutedEventArgs e)
        {
            ///getting a data from the datagrid cell is very tricky
            ///first get datagridcellinfo from selectedcells list
            ///second convert datagridcell to cellcontent
            ///third typecast cellcontect to textblock
            ///forth get text from textblock which will be your required
            ///data
            ///
            ///donot try to use selectedItem as if your are making new 
            ///entry. it will not exist and throw exception

            List<DataGridCellInfo> cells = drugLedger.SelectedCells.ToList();
            string[] newData = new string[5];
            for (int i = 0; i < 5; i++)
            {
                var cellContent = cells[i].Column.GetCellContent(cells[i].Item);
                TextBlock cellText = (TextBlock)cellContent;
                newData[i] = cellText.Text;
                //MessageBox.Show(newData[i] + Environment.NewLine + i.ToString());
            }

            if (string.IsNullOrEmpty(newData[0].ToString().Trim()))
            {
                string cString = @"insert into mainLedger(name,manufacturer,Cost,quantity) values ('" +
                newData[1].ToString() + "','" +
                newData[2].ToString() + "','" +
                newData[4].ToString() + "','" +
                newData[3].ToString() + "');";

                //MessageBox.Show(cString);
                addDatatoTable(cString);
            }
            else
            {
                //MessageBox.Show("Possible Duplicate Entry")
            }

        }

        private void deleteEntry(object sender, RoutedEventArgs e)
        {
            if (validateEntry())
            {
                DataRowView? row = drugLedger.SelectedItem as DataRowView;
                string cString = @"DELETE FROM mainLedger WHERE id='" + row[0].ToString() + "';";
                //MessageBox.Show(cString);
                addDatatoTable(cString);
            }
        }

        public void viewDrugData(object sender, RoutedEventArgs evt)
        {
            if (validateEntry())
            {
                DataRowView? row = drugLedger.SelectedItem as DataRowView;
                string idQuery = @"SELECT * FROM mainLedger WHERE id='" + row[0].ToString() + "';";
                DataTable dTable = new DataTable();
                try
                {
                    connection = new SqlConnection(connectionString);
                    dataAdapter = new SqlDataAdapter(idQuery, connectionString);
                    dataAdapter.Fill(dTable);
                    connection.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("valid data error" +
                     Environment.NewLine + e.Message +
                     Environment.NewLine + e.Source,
                     "Error",
                     MessageBoxButton.OK,
                     MessageBoxImage.Error
                     );
                }
                drugView dv = new drugView(dTable);
                dv.ShowDialog();
            }
        }
        public void addDrugData(object sender, RoutedEventArgs evt)
        {
            viewEntry(this,new RoutedEventArgs());
        }
    }
}
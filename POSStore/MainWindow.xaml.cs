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
        public string connectionString = "Data Source=ENG-RNR-05;Initial Catalog = DSPOS; Integrated Security = True";
        public string queryString = "SELECT * from mainLedger";
        public SqlConnection connection;
        public SqlDataAdapter dataAdapter;
        public MainWindow()
        {
            InitializeComponent();

            connection = new SqlConnection(connectionString);
            dataAdapter = new SqlDataAdapter(queryString, connectionString);            
            DataTable dTable = new DataTable();
            connection.Close();
            dataAdapter.Fill(dTable);
            drugLedger.DataContext = dTable.DefaultView;
            
        }

        private void drugLedger_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    DataRowView row = (DataRowView)drugLedger.SelectedItem;
                    string s = row.Row.ItemArray[3].ToString();
                    MessageBox.Show(s);
                    //MessageBox.Show(drugLedger.SelectedCells.Count.ToString());
                    //for(int i = 1; i < 5;i++) { MessageBox.Show(row[i].ToString()); }
                    
                    if (!(string.IsNullOrEmpty(row[1].ToString()) && string.IsNullOrEmpty(row[2].ToString()) && string.IsNullOrEmpty(row[5].ToString()) && string.IsNullOrEmpty(row[8].ToString())))
                    {
                        //MessageBox.Show(row.Row.ItemArray.Length.ToString());
                        string cString = @"insert into mainLedger(name,manufacturer,Cost,quantity) values ('" +
                         row["name"].ToString() + "','" +
                         row["manufacturer"].ToString() + "','" +
                         row["Cost"].ToString() + "','" +
                         row["quantity"].ToString() + "');";
                        MessageBox.Show(cString);
                        SqlCommand cmd = new SqlCommand(cString, connection);
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        dataAdapter = new SqlDataAdapter(queryString, connectionString);
                        DataTable dTable = new DataTable();                        
                        dataAdapter.Fill(dTable);
                        drugLedger.DataContext = dTable.DefaultView;
                        connection.Close();

                    }
                }
            }
            catch (Exception )
            {

            }
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
            connection.Close();
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
            for (int i = 1; i < 5; i++)
            {
                var cellContent = cells[i].Column.GetCellContent(cells[i].Item);
                TextBlock cellText = (TextBlock)cellContent;
                newData[i] = cellText.Text;
                //MessageBox.Show(newData[i] + Environment.NewLine + i.ToString());
            }

            string cString = @"insert into mainLedger(name,manufacturer,Cost,quantity) values ('" +
            newData[1].ToString() + "','" +
            newData[2].ToString() + "','" +
            newData[3].ToString() + "','" +
            newData[4].ToString() + "');";

            //MessageBox.Show(cString);
            addDatatoTable(cString);

        }

        private void deleteEntry(object sender, RoutedEventArgs e)
        {            
            DataRowView row = drugLedger.SelectedItem as DataRowView;
            string cString = @"DELETE FROM mainLedger WHERE id='" + row[0].ToString() + "';";
            //MessageBox.Show(cString);
            addDatatoTable(cString);
        }
    }
}

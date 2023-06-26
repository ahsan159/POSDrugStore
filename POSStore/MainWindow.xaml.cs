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
    }
}

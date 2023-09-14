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
    class sqlWrapper
    {
        public string connectionString = "Data Source=ENG-RNR-05;Initial Catalog = DSPOS; Integrated Security = True";
        // public string connectionString = "Data Source=AHSAN-PC\\SQLExpress;Initial Catalog=DSPOS;Integrated Security=True;Pooling=False";        
        public string lastCommand { get; set; }
        public string commandType { get; set; }
        public string commandStatus { get; set; }
        public string errorMessage = string.Empty;
        private SqlConnection connection;
        private static sqlWrapper instance = null;

        public static sqlWrapper getInstance()
        {
            if(instance==null)
            {
                instance = new sqlWrapper();
                return instance;
            }
            return instance;
        }

        public sqlWrapper()
        {
            connection = new SqlConnection(connectionString);
            lastCommand = string.Empty;
            commandType = string.Empty;
        }
        public List<string> columnList(string tableName)
        {
            // get the list of column from table
            List<string> list = new List<string>();
            DataTable dt = new DataTable();
            string commandString = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" +
                tableName + "';";
            lastCommand = commandString;
            commandType = "QUERY";
            try
            {
                SqlCommand cmd = new SqlCommand(commandString, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
                connection.Close();
                commandStatus = "Success";
            }
            catch (Exception exp)
            {
                errorMessage = exp.Message;
                commandStatus = "Error";
                connection.Close();
                return null;
            }
            connection.Close();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr.ItemArray[0].ToString());
            }
            return list;
        }
        public int itemCount(string tableName)
        {
            int count = -1;
            string commandString = @"SELECT COUNT(*) FROM " + tableName + ";";
            lastCommand = commandString;
            commandType = "QUERY";
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(commandString, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(dt);
                commandStatus = "Success";
            }
            catch (Exception exp)
            {
                errorMessage = exp.Message;
                commandStatus = "Error";
                connection.Close();
                return -1;
            }
            connection.Close();
            string countString = dt.Rows[0].ItemArray[0].ToString();
            count = int.Parse(countString);
            return count;
        }
        public DataTable getCustumerData()
        {
            return executeBasicQuery("Select Customer,Contact from invoiceLedger where len(Customer)>0;");
        }
        public void executeNonQuery(string commandString)
        {
            /// this will execute the`command that will not return any thing like
            /// deleting and creating tables and data in the tables
            try
            {                
                SqlCommand cmd = new SqlCommand(commandString, connection);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                commandType = "NQUERY";
                commandStatus = "Success";

            }
            catch (Exception exp)
            {
                errorMessage = exp.Message;
                commandStatus = "Error";
                //MessageBox.Show(exp.Message, "Error");
                connection.Close();
            }
        }
        public DataTable executeQuery(string tableName, List<string> columnList = null)
        {
            DataTable sqlTable = new DataTable();            
            string commandString = @"SELECT * FROM " + tableName + ";";
            if (columnList!=null)
            {
                commandString= @"SELECT " + string.Join(",",columnList) + " FROM " + tableName + ";";
            }
            //MessageBox.Show(commandString);
            SqlCommand cmd = new SqlCommand(commandString, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //MessageBox.Show(commandString);
            adapter.Fill(sqlTable);
            connection.Close();
            return sqlTable;
        }
        public DataTable executeQuery(string tableName, string columns)
        {
            DataTable sqlTable = new DataTable();
            string commandString = string.Empty;
            SqlCommand cmd = new SqlCommand(commandString, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(sqlTable);
            connection.Close();
            return sqlTable;
        }
        public DataTable getProductData(string id)
        {
        
                return executeBasicQuery(@"SELECT * FROM mainLedger WHERE id='" + id + "';");
            
        }
        public DataTable executeBasicQuery(string commandString)
        {
            DataTable sqlTable = new DataTable();
            //string commandString = string.Empty;
            SqlCommand cmd = new SqlCommand(commandString, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(sqlTable);
            connection.Close();
            return sqlTable;
        }
        public void deleteTable(string tableName)
        {
            string commandString = @"DELETE TABLE " + tableName + ";";
            executeNonQuery(commandString);
        }
        public DataTable getTable(string tableName)
        {
            return executeQuery(tableName);
        }
        
        public int getStockQuantity(string id)
        {
            string query = @"SELECT quantity from " +
                "mainLedger " +
                "where id='" + id + "';";
            DataTable dt = executeBasicQuery(query);
            return int.Parse(dt.Rows[0]["quantity"].ToString());            
        }
        public void updateQuantity(string id, string updated)
        {
            string query = @"UPDATE " + " mainLedger " +
                "SET quantity='" + updated + "' " +
                " WHERE id='" + id + "';";
            executeNonQuery(query);

        }
        public void updateQuantity(string id, int updated)
        {
            updateQuantity(id, updated.ToString());
        }
        public void tableExist(string tableName)
        {

        }
    }
}

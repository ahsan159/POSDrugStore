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
        //public string connectionString = "Data Source=ENG-RNR-05;Initial Catalog = DSPOS; Integrated Security = True";
        public string connectionString = "Data Source=AHSAN-PC\\SQLExpress;Initial Catalog=DSPOS;Integrated Security=True;Pooling=False";
        public string queryString = "SELECT * from mainLedger";
        public string selectValueCombo;
        public SqlConnection connection;
        public SqlDataAdapter dataAdapter;
        public List<string> dList = new List<string>();
        public DataTable GridCollection = new DataTable();
        public List<string> qList = new List<string>() { "Ahsan", "Ehsan" };
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
            GridCollection.Columns.Add("qty", typeof(string));
            GridCollection.Columns.Add("cost", typeof(string));
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
            string cString = "SELECT DISTINCT(name) FROM mainLedger;";
            connection = new SqlConnection(connectionString);
            dataAdapter = new SqlDataAdapter(cString, connection);
            DataTable dTable = new DataTable();
            dataAdapter.Fill(dTable);
            connection.Close();
            List<string> list = dTable.AsEnumerable().Select(c => c.Field<string>("name")).ToList();
            return list;
        }

        private void saleTable_Selected(object sender, SelectedCellsChangedEventArgs e)
        {
            //int i = 0;
            ////List<DataGridCellInfo> cells = saleTable.SelectedCells.ToList();            
            ////ComboBox cellContent = cells[i].Column.GetCellContent(cells[i].Item) as ComboBox;            
            if(saleTable.Items.Count-1 == saleTable.SelectedIndex)
            {
                DataRow dr = GridCollection.NewRow();
                GridCollection.Rows.Add(dr);
            }

            dList = getDrugList();
            (saleTable.Columns[0] as DataGridComboBoxColumn).ItemsSource = dList;

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
            ComboBox cb = sender as ComboBox;
            //MessageBox.Show(cb.SelectedIndex.ToString() + Environment.NewLine + 
            //    dList[cb.SelectedIndex]);
            List<DataGridCellInfo> cells = saleTable.SelectedCells.ToList();
            TextBlock tb = cells[3].Column.GetCellContent(cells[3].Item) as TextBlock;
            tb.Text = "My data is " + dList[cb.SelectedIndex];
        }

        private void deleteDataRow(object sender, RoutedEventArgs e)
        {
            GridCollection.Rows.RemoveAt(saleTable.SelectedIndex);
        }
        private void closePOS(object sender, RoutedEventArgs e)
        {
            connection.Close();
            GridCollection.WriteXml("backupdata.xml");
            this.Close();
        }
    }
}

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
    public partial class dashBoard : Window
    {
        public DataTable newSaleCollection { get; set; } = new DataTable("NewSale");
        string saleTableName = string.Empty;
        public List<string> drugListComboItems { get; set; } = new List<string>();
        public List<int> drugListComboID { get; set; } = new List<int>();
        public string selectValueBind { get; set; }
        public int indexSelected { get; set; } = 0;
        public string selectedProduct { get; set; } = string.Empty;
        public int selectedComboIndex { get; set; } = 0;
        public void initializeNewSale()
        {
            string dateCode = DateTime.Now.ToString("yyyyMMdd");
            invoiceNo.Text = @"Invoice\" + dateCode + @"\" + dWrap.itemCount("invoiceLedger").ToString();
            saleTableName = invoiceNo.Text.Replace(@"\", "_");
            populateDrugListCombo();
            saleTableSQL();


            newSaleCollection.Columns.Add("Name", typeof(string));
            newSaleCollection.Columns.Add("Quantity", typeof(string));
            newSaleCollection.Columns.Add("Price", typeof(string));
            newSaleCollection.Columns.Add("Discount100", typeof(string));
            newSaleCollection.Columns.Add("Discount", typeof(string));
            newSaleCollection.Columns.Add("Total", typeof(string));
            newSaleCollection.Columns.Add("SaleTax", typeof(string));
            newSaleCollection.Columns.Add("ID", typeof(string));
            newSaleCollection.Columns.Add("Stock", typeof(string));
            DataRow dr = newSaleCollection.NewRow();
            newSaleCollection.Rows.Add(newSaleCollection.NewRow());
            //dr["Discount"] = "Data1";
            //newSaleCollection.Rows.Add(dr);
            //DataRow dr2 = newSaleCollection.NewRow();
            //dr2["Discount"] = "Data2";
            //newSaleCollection.Rows.Add(dr2);
            newSaleDataGrid.ItemsSource = newSaleCollection.DefaultView;
            //MessageBox.Show(drugListComboItems.Count.ToString()); 

            (newSaleDataGrid.Columns[0] as DataGridComboBoxColumn).ItemsSource = drugListComboItems;
            // MessageBox.Show("I am called");

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
            dWrap.executeNonQuery(query);
            //MessageBox.Show(sWrap.errorMessage);
        }
        private void newSaleTable_SelectionChanged(object sender, SelectedCellsChangedEventArgs evt)
        {
            //List<DataGridCellInfo> cellInfo = evt.AddedCells. Select(c=>c.Column.Header.ToString().Equals("Product"));
            
            try
            {
                if (newSaleDataGrid.SelectedIndex+1 >= newSaleCollection.Rows.Count)
                {
                    newSaleCollection.Rows.Add(newSaleCollection.NewRow());
                }
            }
            catch (Exception e) { }
        }

        private void populateDrugListCombo()
        {
            DataTable dt = dWrap.executeBasicQuery("SELECT DISTINCT(name),id FROM mainLedger");
            drugListComboItems = dt.Rows.Cast<DataRow>().Select(r => r.Field<string>("name")).ToList();
            drugListComboID = dt.Rows.Cast<DataRow>().Select(r => r.Field<int>("id")).ToList();
        }

        private void deleteDataRow(object sender, RoutedEventArgs evt)
        {

        }

        private void checkOut(object sender, RoutedEventArgs evt)
        {
            MessageBox.Show(indexSelected.ToString() + Environment.NewLine + 
                selectedProduct);
        }
        private void comboFocused(object sender, RoutedEventArgs evt )
        {
            
        }
        private void comboSelected(object sender, RoutedEventArgs evt)
        {
            int index = (sender as ComboBox).SelectedIndex;
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Name"] = drugListComboItems[index];
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Discount"] = "0";
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["SaleTax"] = "143";
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["ID"] = drugListComboID[index];
        }
        private DataGridCell getCell(DataGridCellInfo info)
        {            
            var cellContent =  info.Column.GetCellContent(info.Item);
            if(cellContent != null)
            {
                return cellContent.Parent as DataGridCell;
            }
            return null;

        }
    }
}

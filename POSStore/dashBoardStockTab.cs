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
        public DataTable stockCollection { get; set; } = new DataTable("stockTable");
        public string quantitytobeAdded { get; set; }
        public string retailPriceT { get; set; }
        public string purchasePriceT { get; set; }
        public List<string> drugListBinding { get; set; }
        public string selectDrug { get; set; }
        public int selectDrugIndex { get; set; } = 0;

        public List<string> drugListID = new List<string>();

        private void initializeStockTableTab()
        {
            DataTable drugList = dWrap.executeQuery("mainLedger", new List<string>() { "name", "id" });
            //drugSelection.ItemsSource = drugList.AsEnumerable().Select(r => r.Field<string>("name")).ToList();
            drugListBinding = drugList.Rows.Cast<DataRow>().Select(r => r.ItemArray[0].ToString()).ToList<string>();
            drugListID = drugList.Rows.Cast<DataRow>().Select(r => r.ItemArray[1].ToString()).ToList();
        }
        private void updateStockBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = @"select quantity from " + " mainLedger " + " where id='" +
                    drugListID[selectDrugIndex].ToString()+"';";
                //MessageBox.Show(query);
                string previousQuantity = dWrap.executeBasicQuery(query).Rows[0]["quantity"].ToString();
                int pQ = int.Parse(previousQuantity);
                int aQ = int.Parse(quantitytobeAdded);
                DataRow dr = stockCollection.NewRow();
                dr["ProductID"] = drugListID[selectDrugIndex].ToString();
                dr["QuantityAdded"] = aQ.ToString();
                dr["Purchase"] = purchasePriceT;
                dr["Retail"] = retailPriceT;
                dr["Added"] = DateTime.Today.ToString("yyyy-MM-dd");
                stockCollection.Rows.Add(dr);

                uploadToDB();
                // I still need to update retail price.
                string query2 = "Update mainLedger set quantity='" + 
                    (aQ+pQ).ToString() + "' where id='" + 
                    drugListID[selectDrugIndex].ToString() + "';";
                dWrap.executeNonQuery(query2);

                addedQuantity.Clear();
                retailPrice.Clear();
                purchasePrice.Clear();
                drugSelection.Focus();

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message + Environment.NewLine +
                    exp.Source + Environment.NewLine +
                    exp.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void uploadToDB()
        {
            List<string> fields = stockCollection.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList<string>();
            fields.RemoveAt(0);
            foreach (DataRow dataRow in stockCollection.Rows)
            {
                if (string.IsNullOrEmpty(dataRow["Sr"].ToString()))
                {
                    List<string> fvalues = dataRow.ItemArray.Select(i=>i.ToString()).ToList();
                    fvalues.RemoveAt(0);
                    string query = @"INSERT INTO stockTable (" +
                        string.Join(",", fields ) +
                        @") VALUES ('" +
                        string.Join("','", fvalues) +
                        "');";
                    //MessageBox.Show(query);
                    sqlWrapper Wrap = sqlWrapper.getInstance();
                    Wrap.executeNonQuery(query);
                    if (Wrap.commandStatus != "Success")
                    {
                        MessageBox.Show(Wrap.errorMessage + Environment.NewLine +
                            query);
                    }
                }
            }
        }
        private void viewDrugList(object sender, RoutedEventArgs e)
        {
            drugView dv = new drugView(true);
            dv.ShowDialog();

        }

        private void drugSelection_KeyDown(object sender, KeyEventArgs e)
        {
            ComboBox cbox = sender as ComboBox;
            cbox.IsDropDownOpen = true;
            //if(e.Key==Key.Tab)
            //{
            //    drugSelection.IsDropDownOpen = false;
            //}
        }

        private void drugSelection_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox cbox = sender as ComboBox;
            cbox.IsDropDownOpen = false;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }
        private void stockTable_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}

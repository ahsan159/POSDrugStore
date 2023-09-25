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
        public List<string?> drugListComboItems { get; set; } = new List<string?>();
        public List<int> drugListComboID { get; set; } = new List<int>();
        public string selectValueBind { get; set; }
        public int indexSelected { get; set; } = 0;
        public string selectedProduct { get; set; } = string.Empty;
        public int selectedComboIndex { get; set; } = 0;
        public string nsCustomerName { get; set; } = string.Empty;
        public string nsCustomerContact { get; set; } = string.Empty;
        public string nsSaleTableName { get; set; } = string.Empty;
        public string currentProductName { get; set; } = string.Empty;
        public string currentProductPrice { get; set; } = string.Empty;
        public string currentProductStock { get; set; } = string.Empty;
        public string nsGrandTotal { get; set; } = "";
        public void initializeNewSale()
        {
            string dateCode = DateTime.Now.ToString("yyyyMMddhhmmss");
            // invoiceNo.Text = @"Invoice\" + dateCode + @"\" + dWrap.itemCount("invoiceLedger").ToString();
            nsSaleTableName = @"Invoice\" + dateCode + @"\" + dWrap.itemCount("invoiceLedger").ToString();
            saleTableName = nsSaleTableName.Replace(@"\", "_");
            populateDrugListCombo();

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
            /// this method create a new unique table for every invoice
            /// generated same table name is saved in invoiceledger
            string query = @"CREATE TABLE " + saleTableName +
                            @" (
                           [Sr] INT NOT NULL IDENTITY(1,1),
                           [Name] VARCHAR(50) NOT NULL,
                           [Quantity] INT NOT NULL,
                           [Price] REAL NOT NULL,
                           [Discount100]  REAL NULL,
                           [Discount] REAL NULL,
                           [SaleTax] REAL NULL,
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
                validateRows();
                totalCost.Content = grandTotal().ToString();
                if (newSaleDataGrid.SelectedIndex >= newSaleCollection.Rows.Count)
                {
                    newSaleCollection.Rows.Add(newSaleCollection.NewRow());
                }
                drugName.Content = newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Name"].ToString();
                drugPrice.Content = newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Price"].ToString();
                drugStock.Content = newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Stock"].ToString();
            }
            catch (Exception e) { }
        }

        private void populateDrugListCombo()
        {
            DataTable dt = dWrap.executeBasicQuery("SELECT DISTINCT(name),id FROM mainLedger");
            drugListComboItems = dt.Rows.Cast<DataRow>().Select(r => r.Field<string>("name")).ToList();
            drugListComboID = dt.Rows.Cast<DataRow>().Select(r => r.Field<int>("id")).ToList();
            MessageBox.Show(drugListComboItems.Count.ToString(), "Populate", MessageBoxButton.OK);
        }

        private void deleteDataRow(object sender, RoutedEventArgs evt)
        {

        }

        private void checkOut(object sender, RoutedEventArgs evt)
        {
            executeSale();
        }
        private void comboFocused(object sender, RoutedEventArgs evt)
        {

        }
        private void comboSelected(object sender, RoutedEventArgs evt)
        {
            int index = (sender as ComboBox).SelectedIndex;

            DataTable dt = dWrap.getProductData(drugListComboID[index].ToString());
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Name"] = drugListComboItems[index];
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Discount100"] = 0;
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Discount"] = 0;
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Price"] = dt.Rows[0]["Cost"];
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["ID"] = drugListComboID[index];
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["SaleTax"] = 0;
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Stock"] = 0;

            drugName.Content = newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Name"].ToString();
            drugPrice.Content = newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Price"].ToString();
            drugStock.Content = newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Stock"].ToString();


            try
            {

                validateRows();
                totalCost.Content = grandTotal().ToString();
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.Message);
            }
        }
        private void paidTotal_KeyDown(object sender, KeyEventArgs e)
        {
            double gTotal = grandTotal();
            double discount = 0;
            double paid = 0;
            if (!string.IsNullOrEmpty(discountTotal.Text.ToString()))
            {
                bool b = double.TryParse(discountTotal.Text, out discount);
                if (!b)
                { discount = 0; }
            }
            if (!string.IsNullOrEmpty(paidTotal.Text))
            {
                bool b = double.TryParse(paidTotal.Text, out paid);
                if (!b)
                { paid = 0; }
            }

            balanceTotal.Text = (paid + discount - gTotal).ToString();
            if (sender == this.paidTotal && e.Key == Key.Enter)
            {
                checkOut(this, new RoutedEventArgs());
            }
        }
        //private DataGridCell getCell(DataGridCellInfo info)
        //{
        //    var cellContent = info.Column.GetCellContent(info.Item);
        //    if (cellContent != null)
        //    {
        //        return cellContent.Parent as DataGridCell;
        //    }
        //    return null;

        //}
        private void validateRows()
        {
            foreach (DataRow dr in newSaleCollection.Rows)
            {
                validateSingleRow(dr);
            }
            grandTotal();
        }
        private void validateSingleRow(DataRow dr)
        {
            if (!string.IsNullOrEmpty(dr["Name"].ToString()))
            {
                double price = double.Parse(dr["Price"].ToString());
                double quantity = double.Parse(dr["Quantity"].ToString());
                double discount = 0;
                if (!string.IsNullOrEmpty(dr["Discount100"].ToString()))
                {
                    discount = double.Parse(dr["Discount100"].ToString()) * price * quantity / 100;
                    dr["Discount"] = discount.ToString();
                }
                else if (!string.IsNullOrEmpty(dr["Discount"].ToString()))
                {
                    discount = double.Parse(dr["Discount"].ToString());
                }
                double total = price * quantity - discount;
                dr["Total"] = total.ToString();
            }
        }
        public double grandTotal()
        {
            double total = 0;
            foreach (DataRow dr in newSaleCollection.Rows)
            {
                if (!string.IsNullOrEmpty(dr["Name"].ToString()))
                {
                    total += double.Parse(dr["Total"].ToString());
                }
            }
            // MessageBox.Show("Grand Total: " + total.ToString());
            return total;
        }
        private void executeSale()
        {
            string cUser = "ahsan";
            try
            {
                if (balanceTotal.Text.IndexOf('-') >= 0)
                {
                    //MessageBox.Show("balance negative");
                    return;
                }
                validateRows();
                saleTableSQL();
                string[] headerName = newSaleCollection.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
                foreach (DataRow dr in newSaleCollection.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["Name"].ToString()))
                    {
                        string[] rowData = dr.ItemArray.Cast<string>().ToArray();
                        string qString = "INSERT INTO " + saleTableName + "(" +
                            string.Join(",", headerName) + ") values('" +
                            string.Join("','", rowData) + "');";
                        dWrap.executeNonQuery(qString);
                        int currentQuantity = dWrap.getStockQuantity(dr["ID"].ToString());
                        int saleQuantity = int.Parse(dr["Quantity"].ToString());
                        dWrap.updateQuantity(dr["ID"].ToString(), currentQuantity - saleQuantity);
                    }
                }
                string invoiceString = "INSERT INTO " + " invoiceLedger " +
                    " (Invoice,UserName,Customer,Contact,Total,Discount,Payment,Balance,DrugCount,CheckoutDate,CheckoutTime,DBName) " +
                    " values ('" +
                    invoiceNo.Text + "','" +
                    cUser + "','" +
                    customerName.Text + "','" +
                    contactNo.Text + "','" +
                    totalCost.Content + "','" +
                    discountTotal.Text + "','" +
                    paidTotal.Text + "','" +
                    balanceTotal.Text + "','" +
                    newSaleCollection.Rows.Count.ToString() + "','" +
                    DateTime.Now.ToString("yyyy-MM-dd") + "','" +
                    DateTime.Now.ToString("hh:mm:ss") + "','" +
                    saleTableName + "');";
                // MessageBox.Show(invoiceString);
                dWrap.executeNonQuery(invoiceString);
                //MessageBox.Show(dWrap.commandStatus + Environment.NewLine + dWrap.errorMessage);
                ClearForNewSale();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void ClearForNewSale()
        {
            newSaleCollection.Rows.Clear();
            customerName.Text = "";
            contactNo.Text = "";
            totalCost.Content = "";
            discountTotal.Text = "";
            paidTotal.Text = "";
            balanceTotal.Text = "";
            string dateCode = DateTime.Now.ToString("yyyyMMddhhmmss");
            // invoiceNo.Text = @"Invoice\" + dateCode + @"\" + dWrap.itemCount("invoiceLedger").ToString();
            nsSaleTableName = @"Invoice\" + dateCode + @"\" + dWrap.itemCount("invoiceLedger").ToString();
            saleTableName = nsSaleTableName.Replace(@"\", "_");
            invoiceNo.Text = nsSaleTableName;

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (newSaleTab.IsSelected)
            {
                MessageBox.Show("Selecting New Sale Tab");
                populateDrugListCombo();
            }
        }

        private void newProductEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MessageBox.Show("Enter is pressed");
            }
        }

    }
}

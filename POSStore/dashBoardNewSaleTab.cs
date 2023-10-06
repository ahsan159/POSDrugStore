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
using System.ComponentModel;
using System.DirectoryServices;
using System.Reflection;

namespace POSStore
{
    public partial class dashBoard : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public DataTable newSaleCollection { get; set; } = new DataTable("NewSale");
        string saleTableName = string.Empty;
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
        public string selectedValueBind { get; set; } = string.Empty;
        public void initializeNewSale()
        {
            string dateCode = DateTime.Now.ToString("yyyyMMddhhmmss");
            // invoiceNo.Text = @"Invoice\" + dateCode + @"\" + dWrap.itemCount("invoiceLedger").ToString();
            nsSaleTableName = @"Invoice\" + dateCode + @"\" + dWrap.itemCount("invoiceLedger").ToString();
            saleTableName = nsSaleTableName.Replace(@"\", "_");
            //populateDrugListCombo();

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
            //newSaleCollection.Rows.Add(newSaleCollection.NewRow());
            //dr["Discount"] = "Data1";
            //newSaleCollection.Rows.Add(dr);
            //DataRow dr2 = newSaleCollection.NewRow();
            //dr2["Discount"] = "Data2";
            //newSaleCollection.Rows.Add(dr2);
            //newSaleDataGrid.ItemsSource = newSaleCollection.DefaultView;
            //MessageBox.Show(drugListComboItems.Count.ToString());

            //(newSaleDataGrid.Columns[0] as DataGridComboBoxColumn).ItemsSource = drugListComboItems;

            //ComboBox cb = newSaleDataGrid.Columns[0].GetCellContent(newSaleDataGrid.Items[0]) as ComboBox;
            //cb.Text = "Cells";
            
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

        //private void populateDrugListCombo()
        //{
        //    DataTable dt = dWrap.executeBasicQuery("SELECT DISTINCT(name),id FROM mainLedger");
        //    List<string> sList = dt.Rows.Cast<DataRow>().Select(r => r.Field<string>("name")).ToList(); ;
        //    foreach (string s in sList) {
        //        drugListComboItems.Add(s);
        //            }
        //    drugListComboID = dt.Rows.Cast<DataRow>().Select(r => r.Field<int>("id")).ToList();
        //    //MessageBox.Show(drugListComboItems.Count.ToString(), "Populate", MessageBoxButton.OK);
        //}

        private void deleteDataRow(object sender, RoutedEventArgs evt)
        {
            newSaleCollection.Rows.RemoveAt(indexSelected);
        }

        private void checkOut(object sender, RoutedEventArgs evt)
        {
            //MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\CPOS\");
            
            executeSale();            

        }
        private void DataGridComboFocused(object sender, RoutedEventArgs evt)
        {
            DataRow dr = newSaleCollection.NewRow();
            newSaleCollection.Rows.Add(dr);
            indexSelected = newSaleCollection.Rows.Count - 1;
        }
        private void ComboFocused(object sender, RoutedEventArgs evt)
        {

        }
        private void comboSelected(object sender, RoutedEventArgs evt)
        {
            int index = (sender as ComboBox).SelectedIndex;

            //MessageBox.Show(index.ToString() + Environment.NewLine +
            //                drugListComboID[index].ToString()  + Environment.NewLine +
            //                drugListComboItems[index].ToString() + Environment.NewLine +
            //                drugListComboID.Count() + Environment.NewLine + 
            //                drugListComboItems.Count() + Environment.NewLine
            //                );
            //DataRow dr = productListDT.AsEnumerable().Where(r=>r.Field<string>("id")
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Name"] = drugListComboItems[index];
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Discount100"] = 0;
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Discount"] = 0;
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Price"] = "100";// dt.Rows[0]["Cost"];
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
            //string cUser = "ahsan";
            try
            {
                if(newSaleCollection.Rows.Count<=0)
                {
                    return;
                }
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
                    User + "','" +
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
                dWrap.executeNonQuery(invoiceString);
                //XMLReportGenerator.ReportGenerator rg = new(saleTableName);
                ClearForNewSale();

            }
            catch (Exception e)
            {
                MessageBox.Show("ERR4001:Printing Error " + e.Message + e.StackTrace,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
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
            nsSaleTableName = @"Invoice\" + dateCode + @"\" + dWrap.itemCount("invoiceLedger").ToString();
            saleTableName = nsSaleTableName.Replace(@"\", "_");
            invoiceNo.Text = nsSaleTableName;

        }
        private void newProductQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int q;
                bool b = int.TryParse(newProductQuantity.Text, out q);
                if (b)
                {
                    int index = newProductEntry.SelectedIndex;
                    MessageBox.Show(index.ToString() + Environment.NewLine +
                        drugListComboItems[index] + Environment.NewLine +
                        drugListComboID[index]);
                    string sSearch = newProductEntry.Text.ToUpper();                    
                    DataRow dr = newSaleCollection.NewRow();
                    dr["Name"] = drugListComboItems[index].ToString();
                    dr["Quantity"] = q.ToString();
                    dr["Discount100"] = 0;
                    dr["Discount"] = 0;
                    dr["Price"] = drugListComboCost[index].ToString();
                    dr["ID"] = drugListComboID[index].ToString();
                    dr["SaleTax"] = 0;
                    dr["Stock"] = 0;
                    newSaleCollection.Rows.Add(dr);
                    try
                    {

                        validateRows();
                        totalCost.Content = grandTotal().ToString();
                        newProductEntry.Text = "";
                        newProductQuantity.Text = "";
                    }
                    catch (Exception exp)
                    {
                        // MessageBox.Show(e.Message);
                    }
                }
            }
        }
        private void newProductEntry_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("Key down");
            ComboBox c = sender as ComboBox;
            c.IsDropDownOpen = true;
            if (e.Key != Key.Enter)
            {
                if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.D0 && e.Key <= Key.D9)) // accept only letters
                {
                    //MessageBox.Show(e.Key.ToString(), "E1");
                    string sSearch = (newProductEntry.Text + e.Key.ToString()).ToUpper();
                    drugListComboItems = productListDT.AsEnumerable().Where(r => r.Field<string>("name").ToUpper().StartsWith(sSearch)).Select(r => r.Field<string>("name")).ToList();
                    drugListComboID = productListDT.AsEnumerable().Where(r => r.Field<string>("name").ToUpper().StartsWith(sSearch)).Select(r => r.Field<int>("id")).ToList();
                    drugListComboCost = productListDT.AsEnumerable().Where(r => r.Field<string>("name").ToUpper().StartsWith(sSearch)).Select(r => r.Field<Single>("Cost")).ToList();
                    //(newSaleDataGrid.Columns[0] as DataGridComboBoxColumn).ItemsSource = drugListComboItems;
                }
                else
                {
                    //MessageBox.Show(e.Key.ToString(), "E2");
                }
                //selectDrug = drugListComboItems[selectDrugIndex];
            }
            else if (e.Key == Key.Enter)
            {
                newProductQuantity.Focus();
            }
        }
        private void refreshNewSale()
        {
            //MessageBox.Show("updating");
            //DataTable dt = dWrap.executeBasicQuery("SELECT DISTINCT(name),id FROM mainLedger");
            drugListComboItems = productListDT.Rows.Cast<DataRow>().Select(r => r.Field<string>("name")).ToList(); ;
            drugListComboID = productListDT.Rows.Cast<DataRow>().Select(r => r.Field<int>("id")).ToList();
            //(newSaleDataGrid.Columns[0] as DataGridComboBoxColumn).ItemsSource = drugListComboItems;
        }

    }
}

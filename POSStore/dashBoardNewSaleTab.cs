﻿using System;
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
                if (newSaleDataGrid.SelectedIndex >= newSaleCollection.Rows.Count)
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
            executeSale();
        }
        private void comboFocused(object sender, RoutedEventArgs evt )
        {

        }
        private void comboSelected(object sender, RoutedEventArgs evt)
        {
            int index = (sender as ComboBox).SelectedIndex;

            DataTable dt = dWrap.getProductData(drugListComboID[index].ToString());
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Name"] = drugListComboItems[index];
            // newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Discount100"] = dt.Rows[0]["Discount100"];
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Price"] = dt.Rows[0]["Cost"];
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["ID"] = drugListComboID[index];
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["SaleTax"] = 11;
            newSaleCollection.Rows[newSaleDataGrid.SelectedIndex]["Stock"] = 12;
            try {

            validateRows();
            }catch(Exception e)
            {
                // MessageBox.Show(e.Message);
            }
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
        private void validateRows()
        {
            foreach(DataRow dr in newSaleCollection.Rows)
            {
                validateSingleRow( dr);
            }
        }
        private void validateSingleRow( DataRow dr)
        {
            if (!string.IsNullOrEmpty(dr["Name"].ToString()))
            {
                double price = double.Parse(dr["Price"].ToString());
                double quantity = double.Parse(dr["Quantity"].ToString());
                double discount = 0;
                if (!string.IsNullOrEmpty(dr["Discount100"].ToString()))
                {
                    discount = double.Parse(dr["Discount100"].ToString())*price*quantity/100;
                    dr["Discount"] = discount.ToString();
                }
                else if (!string.IsNullOrEmpty(dr["Discount"].ToString()))
                {
                    discount = double.Parse(dr["Discount"].ToString());
                }
                double total = price*quantity - discount;
                dr["Total"] = total.ToString();
            }
        }
        private void executeSale()
        {
            try {
            saleTableSQL();
            string[] headerName = newSaleCollection.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            foreach(DataRow dr in newSaleCollection.Rows)
            {
                if (!string.IsNullOrEmpty(dr["Name"].ToString())) {
                string[] rowData = dr.ItemArray.Cast<string>().ToArray();
                string qString = "INSERT INTO " + saleTableName + "(" + 
                    string.Join(",",headerName) + ") values('" + 
                    string.Join("','",rowData) + "');";
                dWrap.executeNonQuery(qString);
                }
                // MessageBox.Show(qString);
            }
            }
            catch(Exception e )
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}

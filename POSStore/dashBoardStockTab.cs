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
        public DataTable stockCollection { get; set; } = new DataTable("stockTable");
        public string quantitytobeAdded { get; set; }
        public string retailPriceT { get; set; }
        public string purchasePriceT { get; set; }
        public List<string> drugListBinding { get; set; }
        public string selectDrug { get; set; }
        public int selectDrugIndex { get; set; } = 0;

        public List<string> drugListID = new List<string>();
        private void updateStockBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRow dr = stockCollection.NewRow();
                dr["ProductID"] = drugListID[selectDrugIndex].ToString();
                dr["QuantityAdded"] = quantitytobeAdded;
                dr["Purchase"] = purchasePriceT;
                dr["Retail"] = retailPriceT;
                dr["Added"] = DateTime.Today.ToString("yyyy-MM-dd");
                stockCollection.Rows.Add(dr);
                addedQuantity.Clear();
                retailPrice.Clear();
                purchasePrice.Clear();
                drugSelection.Focus();

            }
            catch (Exception exp)
            {
                MessageBox.Show(quantitytobeAdded + Environment.NewLine +
                retailPriceT + Environment.NewLine +
                purchasePriceT + Environment.NewLine +
                selectDrug);
                MessageBox.Show(exp.Message + Environment.NewLine +
                    exp.Source + Environment.NewLine +
                    exp.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void uploadToDB()
        {
            List<string> fields = stockCollection.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList<string>();
            foreach (DataRow dataRow in stockCollection.Rows)
            {
                if (string.IsNullOrEmpty(dataRow["Sr"].ToString()))
                {
                    List<string> fSelected = new List<string>();
                    List<string> fValue = new List<string>();
                    foreach (string s in fields)
                    {
                        if (!string.IsNullOrEmpty(dataRow[s].ToString()))
                        {
                            fSelected.Add(s);
                            fValue.Add(dataRow[s].ToString());
                        }
                    }
                    string query = @"INSERT INTO stockTable (" +
                        string.Join(",", fSelected) +
                        @") VALUES ('" +
                        string.Join("','", fValue) +
                        "');";
                    //MessageBox.Show(query);
                    sqlWrapper Wrap = sqlWrapper.getInstance();
                    Wrap.executeNonQuery(query);
                    if (Wrap.commandStatus != "Success")
                    {
                        MessageBox.Show(Wrap.errorMessage + Environment.NewLine +
                            Wrap.commandStatus);
                    }
                }
            }
        }
        private void viewDrugList(object sender, RoutedEventArgs e)
        {
            MainWindow mnWindow = new MainWindow(false);
            mnWindow.ShowDialog();

        }

        private void drugSelection_KeyDown(object sender, KeyEventArgs e)
        {
            ComboBox? cbox = sender as ComboBox;
            cbox.IsDropDownOpen = true;
            //if(e.Key==Key.Tab)
            //{
            //    drugSelection.IsDropDownOpen = false;
            //}
        }

        private void drugSelection_LostFocus(object sender, RoutedEventArgs e)
        {
            ComboBox? cbox = sender as ComboBox;
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

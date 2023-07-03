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

namespace POSStore
{
    /// <summary>
    /// Interaction logic for drugView.xaml
    /// </summary>
    public partial class drugView : Window
    {
        private int sqlID = -1;
        public drugView()
        {
            InitializeComponent();
            sqlID = -1;
        }
        public drugView(DataTable dTable)
        {
            InitializeComponent();
            DataRow dr = dTable.Rows[0];
            sqlID = int.Parse(dr["id"].ToString());
            nameData.Text = dr["name"].ToString();
            formulaData.Text = dr["formula"].ToString();
            quantityData.Text = dr["quantity"].ToString();
            priceData.Text = dr["Cost"].ToString();
            purchaseData.Text = dr["costIn"].ToString();
            manufacturerData.Text = dr["manufacturer"].ToString();
            supplierData.Text = dr["supplier"].ToString();
            //expiryData.Text = dr["expiry"].ToString();
            //MessageBox.Show(dr["expiry"].ToString().Split(' ')[0]); // getting and setting date from sql is proving difficult
            if (string.IsNullOrEmpty(dr["expiry"].ToString()))
            {
                expiryData.SelectedDate = DateTime.Now;
            }
            else
            {
                // tokenize date to remove time data                
                DateTime dt = DateTime.ParseExact(dr["expiry"].ToString().Split(' ')[0], "M/d/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                expiryData.SelectedDate = dt;
            }
            //MessageBox.Show(sqlID.ToString());
        }
        public void update(object sender, RoutedEventArgs rea)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("name", nameData.Text);
            dict.Add("formula", formulaData.Text);
            dict.Add("quantity", quantityData.Text);
            dict.Add("Cost", priceData.Text);
            dict.Add("costIn", purchaseData.Text);
            dict.Add("manufacturer", manufacturerData.Text);
            dict.Add("supplier", supplierData.Text);
            DateTime dt = DateTime.ParseExact(expiryData.Text, "M/d/yyyy", System.Globalization.CultureInfo.CurrentCulture);
            dict.Add("expiry", dt.ToString("yyyy-MM-dd"));
            if (sqlID == -1)
            {
                string values = string.Empty;
                string data = string.Empty;
                foreach (KeyValuePair<string, string> kvp in dict)
                {
                    if (!string.IsNullOrEmpty(kvp.Value.Trim()))
                    {
                        values = values + "," + kvp.Key.ToString();
                        data = data + ",'" + kvp.Value.ToString() + "'";
                    }
                }
                string qString = @"INSERT INTO mainLedger(" +
                                    values.Substring(1, values.Length - 1) + ") values (" +
                                    data.Substring(1, data.Length - 1) + ");";
                MessageBox.Show(qString);
                try
                {
                    addDatatoTable(qString);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + Environment.NewLine + e.Source + Environment.NewLine + e.StackTrace,
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                }

            }
            else
            {
                string updates = string.Empty;
                foreach (KeyValuePair<string, string> kvp in dict)
                {
                    if (!string.IsNullOrEmpty(kvp.Value.Trim()))
                    {
                        updates = updates + " " + kvp.Key + "='" + kvp.Value + "',";
                    }
                }
                string qString = @"UPDATE mainLedger" + Environment.NewLine +
                                  "SET " + updates.Substring(0, updates.Length - 1) + Environment.NewLine +
                                  "WHERE id=" + sqlID.ToString();
                MessageBox.Show(qString);
                try
                {
                    addDatatoTable(qString);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + Environment.NewLine + e.Source + Environment.NewLine + e.StackTrace,
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                }
            }
        }
        public void close(object sender, RoutedEventArgs rea)
        {
            this.Close();
        }

        private void addDatatoTable(string cString)
        {
            string connectionString = "Data Source=ENG-RNR-05;Initial Catalog = DSPOS; Integrated Security = True";
            // string? connectionString = "Data Source=AHSAN-PC\\SQLExpress;Initial Catalog=DSPOS;Integrated Security=True;Pooling=False";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(cString, connection);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}


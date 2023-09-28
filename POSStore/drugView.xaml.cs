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
    /// Interaction logic for drugView.xaml
    /// </summary>
    public partial class drugView : Window
    {
        // string connectionString = "Data Source=ENG-RNR-05;Initial Catalog = DSPOS; Integrated Security = True";
        // string  connectionString = "Data Source=AHSAN-PC\\SQLExpress;Initial Catalog=DSPOS;Integrated Security=True;Pooling=False";
        private int sqlID = -1;
        sqlWrapper sWrap = sqlWrapper.getInstance();
        private Brushes brush;

        public drugView()
        {
            InitializeComponent();
            sqlID = -1;
            Update.IsEnabled = false;            
        }
        public drugView(bool allowEdit = false)
        {
            InitializeComponent();
            Update.IsEnabled = allowEdit;
        }
        public drugView(DataTable dTable, bool allowEdit=false)
        {
            InitializeComponent();
            Update.IsEnabled = allowEdit;
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
        private bool validatedData()
        {
            int error = 0;
            nameData.ClearValue(TextBox.BorderBrushProperty);
            quantityData.ClearValue(TextBox.BorderBrushProperty);
            manufacturerData.ClearValue(TextBox.BorderBrushProperty);
            priceData.ClearValue(TextBox.BorderBrushProperty);
            if (string.IsNullOrEmpty(nameData.Text.Trim()))
            {
                nameData.BorderBrush = Brushes.Red;
                error++;
            }
            if (string.IsNullOrEmpty(quantityData.Text.Trim()))
            {
                quantityData.BorderBrush = Brushes.Red;
                error++;
            }
            if (string.IsNullOrEmpty(manufacturerData.Text.Trim()))
            {
                manufacturerData.BorderBrush = Brushes.Red;
                error++;
            }
            if (string.IsNullOrEmpty(priceData.Text.Trim()))
            {
                priceData.BorderBrush = Brushes.Red;
                error++;
            }
            if (error>0)
            {
                return false;
            }
            return true;
        }
        public void update(object sender, RoutedEventArgs rea)
        {
            if (!validatedData())
            {
                messageLabel.Content = "**Kindly input the required fields";
                return;
            }
            if (!sWrap.IsUniqueDrugName(nameData.Text.Trim()))
            {
                messageLabel.Content = "**Product already exist";
                nameData.BorderBrush = Brushes.Red;
                return;
            }
            nameData.ClearValue(TextBox.BorderBrushProperty);
            quantityData.ClearValue(TextBox.BorderBrushProperty);
            manufacturerData.ClearValue(TextBox.BorderBrushProperty);
            priceData.ClearValue(TextBox.BorderBrushProperty);
            messageLabel.Content = "";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("name", nameData.Text);
            dict.Add("formula", formulaData.Text);
            dict.Add("quantity", quantityData.Text);
            dict.Add("Cost", priceData.Text);
            dict.Add("costIn", purchaseData.Text);
            dict.Add("manufacturer", manufacturerData.Text);
            dict.Add("supplier", supplierData.Text);
            try
            {
                DateTime dt = DateTime.ParseExact(expiryData.Text, "M/d/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                dict.Add("expiry", dt.ToString("yyyy-MM-dd"));
            }
            catch (Exception e) { }
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
                //MessageBox.Show(qString);
                try
                {
                    sWrap.executeNonQuery(qString);
                    this.Close();
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
                // MessageBox.Show(qString);
                try
                {
                    sWrap.executeNonQuery(qString);
                    this.Close();
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
    }
}


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
        private int sqlID = -1;
        public drugView()
        {
            InitializeComponent();
            sqlID = -1;
        }
        public drugView(DataTable dTable)
        {
            InitializeComponent();
            DataRow dr =  dTable.Rows[0];
            sqlID = int.Parse(dr["id"].ToString());
            nameData.Text = dr["name"].ToString();
            formulaData.Text = dr["formula"].ToString();
            quantityData.Text= dr["quantity"].ToString();
            priceData.Text = dr["Cost"].ToString();
            purchaseData.Text = dr["costIn"].ToString();
            manufacturerData.Text = dr["manufacturer"].ToString();
            supplierData.Text = dr["supplier"].ToString();
            expiryData.Text = dr["expiry"].ToString();
        }
        public void update(object sender, RoutedEventArgs rea)
        {
            Dictionary<string,string> dict = new Dictionary<string, string>();
            dict.Add("name",nameData.Text);
            dict.Add("formula",formulaData.Text);
            dict.Add("quantity",quantityData.Text);
            dict.Add("Cost",priceData.Text);
            dict.Add("costIn",purchaseData.Text);
            dict.Add("manufacturer",manufacturerData.Text);
            dict.Add("supplier",supplierData.Text);
            DateTime dt = new DateTime();
            dt.parse
            dict.Add("expiry",

            if(sqlID==-1)
            {

            }
        }
        public void close(object sender, RoutedEventArgs rea)
        {
            this.Close();
        }
    }
}

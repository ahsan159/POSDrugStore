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
        public DataTable customerList { get; set; } = new DataTable();
        public void updateCustomerTable()
        {
            customerList = dWrap.getCustumerData();
        }
        private void refreshCustomers()
        {
            customerList.Rows.Clear();
            DataTable update = dWrap.getCustumerData();
            foreach(DataRow dr in update.Rows)
            {
                customerList.Rows.Add(dr.ItemArray);
            }
        }
    }
}

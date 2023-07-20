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
        public DataTable productListDT { get; set; } = new DataTable("productList");
        public int selectitemIndex { get; set; } = 0;
        public void initializeDrugListTab()
        {
            productListDT = dWrap.getTable("mainLedger");
        }
        public void viewEntry(object sender, RoutedEventArgs evt)
        {
            string idSelected = productListDT.Rows[selectitemIndex]["id"].ToString();
            drugView dv = new drugView(dWrap.executeBasicQuery("SELECT * FROM mainLedger WHERE id='" + idSelected + "';"));
            dv.ShowDialog();
        }
        public void updateEntry(object sender, RoutedEventArgs evt)
        {
            string idSelected = productListDT.Rows[selectitemIndex]["id"].ToString();
            drugView dv = new drugView(dWrap.executeBasicQuery("SELECT * FROM mainLedger WHERE id='" + idSelected + "';"), true);
            dv.ShowDialog();
        }
        public void deleteEntry(object sender, RoutedEventArgs evt)
        {
            dialogYESNO dlyn = new dialogYESNO();
            dlyn.ShowDialog();
            if (dlyn.result == 1)
            {
                string idSelected = productListDT.Rows[selectitemIndex]["id"].ToString();
                dWrap.executeNonQuery("DELETE FROM mainLedger WHERE id='" + idSelected + "';");
                if (dWrap.commandStatus.Equals("Success"))
                {
                    productListDT.Rows.RemoveAt(selectitemIndex);
                }
            }            
            //refresh();
        }
        public void addNewDrug(object sender, RoutedEventArgs evt)
        {
            drugView dv = new drugView(true);
            dv.ShowDialog();
            //productListDT.Reset();
            //productListDT = dWrap.getTable("mainLedger");            
            refresh();
        }
        void refresh()
        {
            productListDT.Rows.Clear();
            DataTable updated = dWrap.getTable("mainLedger");
            foreach (DataRow dr in updated.Rows)
            {
                productListDT.Rows.Add(dr.ItemArray);
            }
        }
    }
}

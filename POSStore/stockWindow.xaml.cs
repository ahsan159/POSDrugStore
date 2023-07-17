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
    /// <summary>
    /// Interaction logic for stockWindow.xaml
    /// </summary>
    public partial class stockWindow : Window
    {
        sqlWrapper dWrap = sqlWrapper.getInstance();
        DataTable stockCollection = new DataTable("stockTable");
        public stockWindow()
        {
            InitializeComponent();
            stockCollection = dWrap.getTable("stockTable");
            DataRow dr = stockCollection.NewRow();
            stockCollection.Rows.Add(dr);
            stockTable.ItemsSource = stockCollection.DefaultView;
            DataTable drugList = dWrap.executeQuery("mainLedger", new List<string>() { "name" });            
            //drugSelection.ItemsSource = drugList.AsEnumerable().Select(r => r.Field<string>("name")).ToList();
            drugSelection.ItemsSource = drugList.Rows.Cast<DataRow>().Select(r => r.ItemArray[0].ToString()).ToList<string>();
        }

        private void stockTable_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void updateStockBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

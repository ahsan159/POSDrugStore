using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POSStore
{
    /// <summary>
    /// Interaction logic for dashBoard.xaml
    /// </summary>
    public partial class dashBoard : Window
    {
        sqlWrapper wrap = sqlWrapper.getInstance();
        Brush defaultColor;
        public dashBoard()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            defaultColor = closeBtn.Background;
            updateDashText();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sideBar.Height = e.NewSize.Height - closeBtn.Height - functionBar.Margin.Top - 10;
            dashBaord.Width = e.NewSize.Width - sideBar.ActualWidth;
        }
        private void closeWindow(object sender, RoutedEventArgs evt)
        {
            this.Close();
        }
        private void stockWindowOpen(object sender, RoutedEventArgs evt)
        {            
            stockWindow sWin = new stockWindow();
            sWin.ShowDialog();
            this.Show();
        }
        private void openDrugList(object sender, RoutedEventArgs evt)
        {
            this.Hide();
            MainWindow mw = new MainWindow();
            mw.ShowDialog();
            this.Show();
            updateDashText();
            clearControlColour();            
        }
        private void openCustomerList(object sender, RoutedEventArgs evt)
        {
            updateDashText();
            clearControlColour();
        }
        private void openSaleList(object sender, RoutedEventArgs evt)
        {
            updateDashText();
            clearControlColour();            
        }
        private void openStockList(object sender, RoutedEventArgs evt)
        {
            updateDashText();
            clearControlColour();
        }
        private void dashBoardOpen(object sender, RoutedEventArgs evt)
        {
            updateDashText();
            clearControlColour();                   
        }
        private void updateDashText()
        {
            int drugCount = wrap.itemCount("mainLedger");
            pBtn.Content = "Products" + Environment.NewLine + "(" + drugCount.ToString() + ")";
            int stockCount = wrap.itemCount("stockTable");
            stockBtn.Content = "Stocks " + Environment.NewLine + "(" + stockCount.ToString() + ")";
            int saleCount = wrap.itemCount("mainLedger");
            saleBtn.Content = "Sales " + Environment.NewLine + "(" + saleCount.ToString() + ")";
            int customerCount = 0;
            cBtn.Content = "Customer " + Environment.NewLine + "(" + customerCount.ToString() + ")";
        }
        private void clearControlColour()
        {
            pBtn.Background = defaultColor;            
        }
    }
}

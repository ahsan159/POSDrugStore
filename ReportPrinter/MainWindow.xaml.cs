using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
using System.Windows.Xps.Packaging;

namespace ReportPrinter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                web.Navigate(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\report.html");
                //MSHTML.IHTMLDocument2 doc = web.Document as MSHTML.IHTMLDocument2;
                //doc.execCommand("Print", true, null);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs evt)
        {
            try
            {
                MSHTML.IHTMLDocument2 doc = web.Document as MSHTML.IHTMLDocument2;
                doc.execCommand("Print", true, null);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
            }
        }
    }
}

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
    /// Interaction logic for dialogYESNO.xaml
    /// </summary>
    /// 
    public partial class dialogYESNO : Window
    {
        public int result = -1;
        public dialogYESNO()
        {
            InitializeComponent();
        }
        public void yesClick(object sender, RoutedEventArgs evt)
        {
            result = 1;
            this.Close();
            
        }
        public void noClick(object sender, RoutedEventArgs evt)
        {
            result = -1;
            this.Close();
        }
    }
}

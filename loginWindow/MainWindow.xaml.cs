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
using Microsoft.Win32;

namespace loginWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        sqlWrapper sWrap = sqlWrapper.getInstance();
        registryData.registryDataClass reg = new registryData.registryDataClass();
        string exeLocation;
        public MainWindow()
        {
            InitializeComponent();            
            exeLocation = reg.getInstallLocation();

        }
        private void loginClick(object sender, RoutedEventArgs evt)
        {
            string usernameString = username.Text.ToString();
            string passwordString = password.Password.ToString();
            string[] str = sWrap.executeLoginQuery("SELECT password,level from loginTable where Name='" + usernameString + "';");
            if (str != null)
            {
                if (passwordString == str[0])
                {
                    reg.setUser(usernameString, str[1]);                    
                    System.Diagnostics.Process.Start(exeLocation);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Incorrent password");
                }
            }
            else
            {
                MessageBox.Show("Cannot find user");
            }


        }
        private void closeClick(object sender, RoutedEventArgs evt)
        {
            this.Close();
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                loginClick(this, new RoutedEventArgs());
            }
        }
    }
}

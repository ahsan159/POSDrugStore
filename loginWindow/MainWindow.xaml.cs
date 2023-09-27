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
using System.Diagnostics;

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
            if (reg.firstRun())
            {
                // if application run for the first time it should setup the environment                
                string exeName = @"..\setup\setupEnvironment.exe";
                System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo(exeName);
                processStartInfo.Verb = "runas";
                processStartInfo.UseShellExecute = true;
                try
                {
                    System.Diagnostics.Process.Start(processStartInfo);
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERR:1001" + Environment.NewLine +
                                e.Message +
                                e.StackTrace,
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                    App.Current.Shutdown();
                }
            }
            else
            {
                // otherwise run the setup as normal and start the localdb server if it is not running
                try
                {
                    string exeCmd = @"sqllocaldb s mssqllocaldb";
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(exeCmd);
                    processStartInfo.UseShellExecute = true;
                    System.Diagnostics.Process p = new();
                    p.StartInfo = processStartInfo;
                    if (p.Start())
                    {
                        MessageBox.Show("sql server started successfully");
                    }
                    else
                    {
                        MessageBox.Show("ERR:1002" + Environment.NewLine + "Unable to start sql server!!! contact administrator",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERR:1003" + Environment.NewLine +
                        e.Message +
                        e.StackTrace,
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    App.Current.Shutdown();
                }
            }
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
                    exeLocation = @"..\POSStore\POSStore.exe";
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
            if (e.Key == Key.Enter)
            {
                loginClick(this, new RoutedEventArgs());
            }
        }
    }
}

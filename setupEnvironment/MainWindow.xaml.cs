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
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;


namespace setupEnvironment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string localdbInstance = "mssqllocaldb";
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                if (!detectSQLInstallation())
                {
                    MessageBox.Show("ERR:2001" + Environment.NewLine + "No SQL Server installations found",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //MessageBox.Show("SQL Installation Detected");                
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = "cmd.exe";
                processStartInfo.Arguments = @" /c sqllocaldb s " + localdbInstance;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process p = new();
                p.StartInfo = processStartInfo;
                p.Start();
                string resultExpected = "LocalDB instance \"" + localdbInstance + "\" started.";
                string result = p.StandardOutput.ReadToEnd().Trim();
                //MessageBox.Show(result + Environment.NewLine + resultExpected);
                if (result.Equals(resultExpected))
                {
                    MessageBox.Show("sql server started successfully");
                    p.Close();
                }
                else
                {
                    p.Close();
                    MessageBox.Show("ERR:2002" + Environment.NewLine + "Unable to start sql server!!! contact administrator",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERR:2003" + Environment.NewLine +
                    e.Message +
                    e.StackTrace,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            // at this point the sql server is started and database creation can move forward
            disp.Content = "You are running the application first time." + Environment.NewLine +
                "Please, wait while we setup your environment for you...";
            if (setupSQL())
            {
                disp.Content = disp.Content + Environment.NewLine + "SQL database setup.... complete";
            }
            else
            {
                disp.Content = disp.Content + Environment.NewLine + "SQL database setup.... Error" + Environment.NewLine +
                    "Please contact your administrator";
                //App.Current.Shutdown();
            }
            // database creation complete move with registry writting for finalizing setup
            if (setupRegistry())
            {
                disp.Content = disp.Content + Environment.NewLine + "Registry setup.... complete";
            }
            else
            {
                disp.Content = disp.Content + Environment.NewLine + "Registry setup.... Error" + Environment.NewLine +
                    "Please contact your administrator";
                //App.Current.Shutdown();
            }
        }
        private bool setupRegistry()
        {
            registryData.registryDataClass reg = new registryData.registryDataClass();
            return reg.writeNewRegistry(false);

        }
        private bool detectSQLInstallation()
        {
            // detect mssql server installation
            //RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MICROSOFT\Microsoft SQL Server");
            //if (regKey != null)
            //{
            //    string installedInstances = regKey.GetValue("InstalledInstances").ToString();
            //    MessageBox.Show(installedInstances.Length.ToString(), "Mesg");
            //    if (installedInstances.Length > 0)
            //    {
            //        return true;
            //    }
            //}

            // detect sql localdb(v11.0/sql server 2012) installation
            RegistryKey regKeyLocalDB = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MICROSOFT\Microsoft SQL Server Local DB\Installed Versions\11.0\", false);
            if (regKeyLocalDB != null)
            {
                string s = regKeyLocalDB.GetValue("ParentInstance").ToString();
                regKeyLocalDB.Close();
                if (s.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool generateDB()
        {
            if (!detectSQLInstallation())
            {
                disp.Content = disp.Content + Environment.NewLine +
                    "Cannot first any sql server installation";
                return false;
            }
            disp.Content = disp.Content + Environment.NewLine +
                "SQL Server Installation validated";
            try
            {
                string connectionString = @"Data Source=(LocalDB)\" + localdbInstance + @"; Integrated Security = True; Initial Catalog = master;";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdStr = @"if not exists(select name  from sys.databases where name='DSPOS')                                  
                                  begin
                                  create database DSPOS;                                  
                                  end";
                SqlCommand cmd = new SqlCommand(cmdStr, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERR:2004" + Environment.NewLine +
                        e.Message +
                        e.StackTrace,
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                return false;
            }
        }
        private bool generateTables()
        {
            string connectionString = @"Data Source=(LocalDB)\" + localdbInstance + @"; Integrated Security = True; Initial Catalog = DSPOS;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string invoiceLedger = @"CREATE TABLE invoiceLedger
                                    (
                                        [Sr] INT IDENTITY(1,1) NOT NULL, 	
                                        [Invoice] VARCHAR(50) NOT NULL, 
                                        [UserName] VARCHAR(50) NULL, 
                                        [Customer] VARCHAR(50) NULL, 
                                        [Contact] VARCHAR(50) NULL, 
	                                    [Total] REAL NULL,
                                        [Discount] REAL NULL, 
	                                    [Payment] REAL NULL, 
                                        [Sale_Tax] REAL NULL, 
	                                    [Balance] REAL NULL,
	                                    [PaymentType] VARCHAR(50) NULL, 
	                                    [DrugCount] INT NULL,
	                                    [CheckoutDate] DATE NULL,
	                                    [CheckoutTime] TIME NULL,
                                        [DBName] VARCHAR(50) NULL
                                    )";
            SqlCommand cmd1 = new SqlCommand(invoiceLedger, connection);
            cmd1.ExecuteNonQuery();

            string mainLedger = @"CREATE TABLE mainLedger (
                                [id]           INT          NOT NULL identity(101,7),
                                [name]         VARCHAR (25) NOT NULL,
                                [manufacturer] VARCHAR (40) NOT NULL,
                                [supplier]     VARCHAR (40) NULL,
                                [costIn]       REAL         NULL,
                                [Cost]         REAL         NOT NULL,
                                [expiry]       DATE         NULL,
                                [formula]      VARCHAR (50) NULL,
                                [quantity]     INT          NOT NULL
                            );";
            SqlCommand cmd2 = new SqlCommand(mainLedger, connection);
            cmd2.ExecuteNonQuery();

            string loginTable = @"CREATE TABLE loginTable (
                                [Sr] INT NOT NULL IDENTITY(1,1),
                                [Name] varchar(50) NOT NULL,
                                [Password] varchar(50) NOT NULL,
                                [Level] varchar(10) NOT NULL Default('User'),
                                [Added] DateTime NOT NULL Default(GetDate())
                                )";
            SqlCommand cmd3 = new SqlCommand(loginTable, connection);
            cmd3.ExecuteNonQuery();

            string stocktable = @"CREATE TABLE stockTable (
                                [Sr] INT NOT NULL IDENTITY(1,1),
                                [ProductID] INT NOT NULL,
                                [QuantityAdded] INT NOT NULL,
                                [Purchase] REAL NULL,
                                [Retail] REAL NULL,
                                [Supplier] varchar(50) NULL,
                                [SupplierContact] varchar(10) NULL,
                                [Added] Date NOT NULL,
                                [Users] VARCHAR(50) NULL
                                )";
            SqlCommand cmd4 = new SqlCommand(stocktable, connection);
            cmd4.ExecuteNonQuery();

            connection.Close();
            return false;
        }
        private bool setupSQL()
        {
            try
            {
                generateDB();
                generateTables();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("ERR:2005" + Environment.NewLine +
                        e.Message +
                        e.StackTrace,
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                return false;
            }
        }
    }
}

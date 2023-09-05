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


namespace setupEnvironment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (setupSQL())
            {
                disp.Content = "SQL database setup.... complete";
            }

            if (setupRegistry())
            {
                disp.Content = disp.Content + Environment.NewLine + "Registry setup.... complete";
            }
            ;
            //detectSQLInstallation();
        }
        private bool setupRegistry()
        {
            registryData.registryDataClass reg = new registryData.registryDataClass();
            return reg.writeNewRegistry(true);
            
        }
        private bool detectSQLInstallation()
        {
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MICROSOFT\Microsoft SQL Server");
            if (regKey != null)
            {
                string installedInstances = regKey.GetValue("InstalledInstances").ToString();
                if (installedInstances.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        //private List<string> detectSQLServers()
        //{

        //}
        private bool generateDB()
        {
            try
            {
                string connectionString = "Data Source=.; Integrated Security = True";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand cmd = new SqlCommand("create database CPOSDB;", connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        private bool generateTables()
        {
            string connectionString = "Data Source=.; Integrated Security = True; Initial Catalog = CPOSDB;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string invoiceLedger = @"CREATE TABLE invoiceLedger
                                    (
                                        --[Sr] INT IDENTITY(1,1) NOT NULL, 	
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
	                                    [CheckoutTime] TIME NULL    
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
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}

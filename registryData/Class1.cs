﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using registryData;

namespace registryData
{
    public class registryDataClass
    {
        string root = @"Software\CompanionPOS\";
        public registryDataClass()
        {

        }
        public bool writeNewRegistry(bool firstRun = false)
        {
            try
            {
                Console.WriteLine("registryWritten");
                createConfigKey();
                createLiscenceKeys();
                createLoginKey();
                createsqlKey();
                createFolderKey();
                if (firstRun)
                {
                    setfirstRun();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private void createLiscenceKeys()
        {
            RegistryKey licKey = Registry.LocalMachine.CreateSubKey(root + @"lic");
            licKey.SetValue("Status", "Normal");
            licKey.SetValue("Key", "NOKEY");
            licKey.SetValue("Expiry", "31-12-2023");
            licKey.Close();
        }
        private void createConfigKey()
        {
            RegistryKey configKey = Registry.LocalMachine.CreateSubKey(root + @"config");
            configKey.SetValue("Customer", "Name");
            configKey.SetValue("CustomerID", "12397");
            configKey.SetValue("InstallLocation", @"C:\Users\muhammadahsan\source\repos\POSStore\POSStore\bin\Debug\netcoreapp3.1\POSStore.exe");
            configKey.SetValue("dataLocation", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\");
            configKey.Close();

        }
        private void createLoginKey()
        {
            RegistryKey loginKey = Registry.CurrentUser.CreateSubKey(root + @"login");
            loginKey.SetValue("Username", "admin");
            loginKey.SetValue("Userlevel", "admin");
            loginKey.SetValue("lastActive", "01-08-2023");
            loginKey.SetValue("Status", "LoginIn");
            loginKey.Close();
            // RegistryKey loginKey1 = Registry.LocalMachine.CreateSubKey(root+@"login\Username",true);
            // loginKey1.SetValue("Username","Ahsan");
            // loginKey1.SetValue(root+@"login\Username","Ahsan");
        }
        private void createsqlKey()
        {
            RegistryKey sqlKey = Registry.CurrentUser.CreateSubKey(root + @"sql");
            sqlKey.SetValue("Connection", @"(LocalDB)\MSSQLLocalDB");
            sqlKey.SetValue("Database", "DSPOS");
            sqlKey.SetValue("Product", "mainLedger");
            sqlKey.SetValue("Stock", "stockTable");
            sqlKey.SetValue("Invoice", "invoiceLedger");
            sqlKey.SetValue("Sale", "saleLedger");
            sqlKey.Close();
        }
        private void createFolderKey()
        {
            RegistryKey folderKey = Registry.CurrentUser.CreateSubKey(root + @"folders");
            folderKey.SetValue("dataLocation", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\CPOS\");
            folderKey.SetValue("InstallLocation", @"C:\Users\muhammadahsan\source\repos\POSStore\POSStore\bin\Debug\netcoreapp3.1\POSStore.exe");
            folderKey.Close();
        }
        public string? getDataFolder()
        {
            var reg = Registry.CurrentUser.OpenSubKey(root + @"folders");
            string? str = string.Empty;
            if (reg != null)
            {
                str = reg.GetValue("dataLocation").ToString();
                reg.Close();
            }
            return str;
        }
        public string? getInsallFolder()
        {
            var reg = Registry.CurrentUser.OpenSubKey(root + @"folders");
            string? str = string.Empty;
            if (reg != null)
            {
                str = reg.GetValue("IntallLocation").ToString();
                reg.Close();
            }
            return str;
        }
        public string getLoginName()
        {
            var reg = Registry.CurrentUser.OpenSubKey(root + @"login");
            if (reg != null)
            {
                //Console.WriteLine(string.Join(",",reg.GetValueNames()));
                //Console.WriteLine(reg.GetValue("Username").ToString());
                return reg.GetValue("Username").ToString();
            }
            return string.Empty;
        }
        public string getLoginStatus()
        {
            var reg = Registry.CurrentUser.OpenSubKey(root + @"login");
            if (reg != null)
            {
            }
            return string.Empty;
        }
        public string getLicenseStatus()
        {
            var reg = Registry.LocalMachine.OpenSubKey(root + @"lic");
            if (reg != null)
            {

            }
            return string.Empty;
        }
        public string getLicenseKey()
        {
            var reg = Registry.LocalMachine.OpenSubKey(root + @"lic");
            if (reg != null)
            {

            }
            return string.Empty;
        }
        public string getLicenseExpiry()
        {
            var reg = Registry.LocalMachine.OpenSubKey(root + @"lic");
            if (reg != null)
            {

            }
            return string.Empty;
        }
        public bool validateLicense()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"Software\CompanionPOS\lic");
            DateTime expiry = DateTime.ParseExact(reg.GetValue("Expiry").ToString(), "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
            DateTime current = DateTime.Now;
            if (reg != null)
            {
                if (reg.GetValue("Status").ToString() == "Normal" && expiry > current)
                {
                    Console.WriteLine("Normal Level");
                    reg.Close();
                    return true;
                }
            }
            return false;
        }
        public void setUser(string user, string level)
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\CompanionPOS\login", true);
            if (regKey != null)
            {
                regKey.SetValue("Username", user);
                regKey.SetValue("Userlevel", level);
                setActivityStatus("LoggedIn");
                regKey.SetValue("lastActive", DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            regKey.Close();
        }
        public string getUser()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\CompanionPOS\login");
            if (regKey != null)
            {
                return regKey.GetValue("Username").ToString();
            }
            return string.Empty;
        }
        public string getLevel()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\CompanionPOS\login");
            if (regKey != null)
            {
                return regKey.GetValue("Userlevel").ToString();
            }
            return string.Empty;
        }
        public string getActivityStatus()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\CompanionPOS\login");
            if (regKey != null)
            {
                return regKey.GetValue("Status").ToString();
            }
            return string.Empty;
        }
        public void setActivityStatus(string status)
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software\CompanionPOS\login", true);
            if (regKey != null)
            {
                regKey.SetValue("Status", status);
                regKey.Close();
            }
        }
        public string getInstallLocation()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"Software\CompanionPOS\config");
            string str = string.Empty;
            if (registryKey != null)
            {
                str = registryKey.GetValue("InstallLocation").ToString();
            }
            return str;
        }
        public bool firstRun()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"Software\CompanionPOS\config");
            if (registryKey != null) // check if registry exist or not
            {
                // is registry exist try to get value of requested installlevel key
                if (registryKey.GetValue("installLevel") != null)
                {
                    // if value exist get value and compare
                    string strInstallLevel = registryKey.GetValue("installLevel").ToString();
                    if (strInstallLevel.Equals("firstRun"))
                    {
                        registryKey.Close();
                        return true;
                    }
                    else
                    {
                        registryKey.Close();
                        return false;
                    }
                }
                else
                {
                    // if value does not exist
                    return true;
                }
            }
            else
            {
                // if registry does not exist
                return true;
            }
        }
        public void setfirstRun()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"Software\CompanionPOS\config", true);
            if (registryKey != null)
            {
                registryKey.SetValue("installLevel", "setupComplete");
                registryKey.Close();
            }
            else
            {
                RegistryKey rKey = Registry.LocalMachine.CreateSubKey(@"Software\CompanionPOS\config");
                rKey.SetValue("installLevel", "firstRun");
                rKey.Close();
            }
        }
    }
}
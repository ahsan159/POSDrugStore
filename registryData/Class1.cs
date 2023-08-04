using Microsoft.Win32;
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

namespace registryData;
public class registryDataClass
{
    string root = @"Software\CompanionPOS\";
    public registryDataClass()
    {

    }
    public void writeNewRegistry()
    {
        createConfigKey();
        createLiscenceKeys();
        createLoginKey();
        createsqlKey();
        Console.WriteLine("registryWritten");
    }
    private void createLiscenceKeys()
    {
        RegistryKey licKey = Registry.LocalMachine.CreateSubKey(root + @"lic");
        licKey.SetValue("Status", "Normal");
        licKey.SetValue("Key", "NOKEY");
        licKey.SetValue("Expiry", "31-12-2023");
    }
    private void createConfigKey()
    {
        RegistryKey configKey = Registry.LocalMachine.CreateSubKey(root + @"config");
        configKey.SetValue("Customer", "Name");
        configKey.SetValue("CustomerID", "12397");
        configKey.SetValue("InstallLocation",@"C:\Users\muhammadahsan\source\repos\POSStore\POSStore\bin\Debug\netcoreapp3.1\POSStore.exe");
        
    }
    private void createLoginKey()
    {
        RegistryKey loginKey = Registry.CurrentUser.CreateSubKey(root + @"login");
        loginKey.SetValue("Username", "Ahsan");
        loginKey.SetValue("Userlevel", "admin");
        loginKey.SetValue("lastActive", "01-08-2023");
        loginKey.SetValue("Status", "LoginIn");
        // RegistryKey loginKey1 = Registry.LocalMachine.CreateSubKey(root+@"login\Username",true);
        // loginKey1.SetValue("Username","Ahsan");
        // loginKey1.SetValue(root+@"login\Username","Ahsan");
    }
    private void createsqlKey()
    {
        RegistryKey sqlKey = Registry.CurrentUser.CreateSubKey(root + @"sql");
        sqlKey.SetValue("Connection", "POS");
        sqlKey.SetValue("Product", "mainLedger");
        sqlKey.SetValue("Stock", "stockTable");
        sqlKey.SetValue("Invoice", "Invoice");
        sqlKey.SetValue("Sale", "saleLedger");
    }

    public string? getLoginName()
    {
        var reg = Registry.CurrentUser.OpenSubKey(root + @"login");
        if (reg != null)
        {
            //Console.WriteLine(string.Join(",",reg.GetValueNames()));
            //Console.WriteLine(reg.GetValue("Username").ToString());
            return reg.GetValue("Username")?.ToString();
        }
        return string.Empty;
    }
    public string? getLoginStatus()
    {
        var reg = Registry.CurrentUser.OpenSubKey(root + @"login");
        if (reg != null)
        {
            return reg.GetValue("Userlevel")?.ToString();
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
            if (reg.GetValue("Status")?.ToString() == "Normal" && expiry > current)
            {
                Console.WriteLine("Normal Level");
                return true;
            }
        }
        return false;
    }
    public void setUser(string user, string level)
    {
        RegistryKey? regKey = Registry.CurrentUser.OpenSubKey(@"Software\CompanionPOS\login",true);
        if (regKey != null)
        {
            regKey.SetValue("Username", user);
            regKey.SetValue("Userlevel", level);

        }
    }
    public string? activityStatus()
    {
        RegistryKey? regKey = Registry.CurrentUser.OpenSubKey(@"Software\CompanionPOS\login");
        if (regKey != null)
        {
            return regKey.GetValue("Status")?.ToString();
        }
        return string.Empty;
    }
    public void updateActivity(string status)
    {
        RegistryKey? regKey = Registry.LocalMachine.OpenSubKey(@"Software\CompanionPOS\login",true);
        if (regKey != null)
        {
            regKey.SetValue("Status", status);
        }
    }
    public string? getInstallLocation()
    {
        RegistryKey? registryKey = Registry.LocalMachine.OpenSubKey(@"Software\CompanionPOS\config");
        string? str = string.Empty; 
        if (registryKey!=null)
        {
            str = registryKey.GetValue("InstallLocation")?.ToString();
        }
        return str;
    }
}
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
using System.Collections.Specialized;
using System.ComponentModel;


// Error Codes start with 5
// should be visible to only admins


namespace POSStore
{

    public partial class dashBoard : Window
    {
        public DataTable ActiveLoginTable { get; set; } = new();
        public int LoginTable_SelectedIndex { get; set; } = 0;
        public List<string> LoginLevelDropDownItems { get; set; } = new List<string>() { "user","admin" };
        public int LoginLevelDropDownIndex { get; set; } = 0;
        public void initializeConfigTab()
        {
            ActiveLoginTable = dWrap.getTable("loginTable");
        }

        private void ConfigTab_Clicked(object sender, RoutedEventArgs evt)
        {
            try
            {
                DataWrapper.sqlWrapper wrapper = new DataWrapper.sqlWrapper();
                DataTable buf = wrapper.getTable("loginTable");
                ActiveLoginTable.Rows.Clear();
                foreach (DataRow dr in buf.Rows)
                {
                    ActiveLoginTable.Rows.Add(dr.ItemArray);
                }
                //MessageBox.Show(ActiveLoginTable.Rows.Count.ToString());
            }
            catch(Exception e)
            {
                MessageBox.Show("Err:5001 " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void DeleteUser(object sender, RoutedEventArgs evt)
        {
            DataRow dr = ActiveLoginTable.Rows[LoginTable_SelectedIndex];
            //MessageBox.Show(dr["name"].ToString());
            dialogYESNO dlyn = new dialogYESNO();
            dlyn.ShowDialog();
            if (dlyn.result == 1)
            {
                string query = @"delete from loginTable where name = '" + dr["name"].ToString() + "'";
                DataWrapper.sqlWrapper wrapper = new();
                wrapper.executeNonQuery(query);
                ConfigTab_Clicked(this, new RoutedEventArgs());
            }
        }
        private void AddUser(object sender, RoutedEventArgs evt)
        {
            if (string.IsNullOrEmpty(newuser.Text))
            {
                return;
            }
            DataRow[] dr = ActiveLoginTable.AsEnumerable().Where(r => r.Field<string>("name").ToUpper() == newuser.Text.ToUpper()).ToArray();
            if (dr.Count()>0)
            {
                MessageBox.Show("Err:5002 Cannot add user name","Info",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else
            {
                if (!string.IsNullOrEmpty(newpassword1.Password) && !string.IsNullOrEmpty(newpassword2.Password))
                {
                    if(newpassword2.Password!=newpassword1.Password)
                    {
                        MessageBox.Show("Err:5001 Password does not match!!!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        string query = @"INSERT INTO loginTable(Name,Password,Level) values('" + newuser.Text + @"','"
                                       + newpassword1.Password + @"','" 
                                       + LoginLevelDropDownItems.ElementAt(LoginLevelDropDownIndex).ToString() + @"');";
                        DataWrapper.sqlWrapper wrapper = new();
                        wrapper.executeBasicQuery(query);
                        ConfigTab_Clicked(this, new RoutedEventArgs());
                        newpassword1.Password = "";
                        newpassword2.Password = "";
                        newuser.Text = "";
                    }
                }
            }            
        }

    }
}
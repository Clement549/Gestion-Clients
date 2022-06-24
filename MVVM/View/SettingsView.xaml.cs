using HelixToolkit.Wpf;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Esilv_BDD.MVVM.View
{
    public partial class SettingsView : UserControl
    {

        public SettingsView()
        {
            InitializeComponent();

            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content.ToString() == Variables_Manager.export_format)
                {
                    comboBox.SelectedValue = item;
                    break;
                }
            }

            if(Variables_Manager.adminLogin == "SERVER=localhost;PORT=3306;"
                                             + "DATABASE = velomax;"
                                             + "UID=root; PASSWORD=root")
            {
                comboBox_admin.SelectedValue = comboBox_admin.Items[0];
            }
            else
            {
                comboBox_admin.SelectedValue = comboBox_admin.Items[1];
            }
        }

        private void ComboBox_SelectionChanged_Admin(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            string _s= cmb.SelectedItem.ToString();
            if (_s.Length > 39)
            {
                string s = _s.Remove(0, 39);

                if (s == "Administrateur")
                {
                    Variables_Manager.adminLogin = "SERVER=localhost;PORT=3306;"
                                                 + "DATABASE = velomax;"
                                                 + "UID=root; PASSWORD=root";
                }
                else if (s == "Utilisateur")
                {

                    Variables_Manager.adminLogin = "SERVER=localhost;PORT=3306;"
                                                 + "DATABASE = velomax;"
                                                 + "UID=bozo; PASSWORD=bozo";
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (cmb.SelectedItem != null)
            {
                string s = cmb.SelectedItem.ToString();
                string format = s.Substring(s.Length - 4);
                if (format == "Json")
                {
                    Variables_Manager.export_format = ".Json";

                    try
                    {
                        string Query = "UPDATE manager SET export_format = '.Json';";
                        MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                        MyConn.Open();
                        MyCommand2.ExecuteReader();
                        MyConn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (format == ".Xml")
                {
                    Variables_Manager.export_format = ".Xml";

                    try
                    {
                        string Query = "UPDATE manager SET export_format = '.Xml';";
                        MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                        MyConn.Open();
                        MyCommand2.ExecuteReader();
                        MyConn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (format == ".Csv")
                {
                    Variables_Manager.export_format = ".Csv";

                    try
                    {
                        string Query = "UPDATE manager SET export_format = '.Csv';";
                        MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                        MyConn.Open();
                        MyCommand2.ExecuteReader();
                        MyConn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}

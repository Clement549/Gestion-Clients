using MySql.Data.MySqlClient;
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

namespace Esilv_BDD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TextBlock moneyTextStatic;
        public MainWindow()
        {
            InitializeComponent();

            loadSQLData();

            moneyTextStatic = moneyText;
        }

        public async void loadSQLData()
        {

            MySqlConnection maConnexion;
            try
            {
                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT * from manager;";
                MySqlCommand command1 = maConnexion.CreateCommand();
                command1.CommandText = requete;

                var reader = await command1.ExecuteReaderAsync();

                string[] valueString = new string[reader.FieldCount];
                while (await reader.ReadAsync())
                {
                    double money = (double)reader["argent"];
                    string export_format = (string)reader["export_format"];

                    Variables_Manager.money = (double)money;
                    Variables_Manager.export_format = export_format;
                }

                moneyText.Text = "Compte: " + string.Format("{0:N}", Variables_Manager.money) + "€";

                await reader.CloseAsync();
                await command1.DisposeAsync();
                await maConnexion.CloseAsync();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }
        }

        /// <summary>
        /// Déplacer la fenetre avec la souris
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            try
            {
                this.DragMove();
            }
            catch(Exception e1) { }
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Esilv_BDD.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour HomeView.xaml
    /// </summary>
    partial class StatsView : UserControl
    {
        public int _today_money = 0;
        public int _today_sellNb = 0;
        public double _today_trend = -15.89;

        public int _week_money = 0;
        public int _week_sellNb = 0;
        public double _week_trend = 2.24;

        public int _average_money = 0;
        public int _average_sellNb = 0;
        public double _average_trend= -7.44;

        public int _bestCustomer_money = 3689;
        public string _bestCustomer_name = "Clément";
        public string _bestCustomer_city = "Paris";

        public StatsView()
        {
            InitializeComponent();

            ComputeStats();

            today_money.Text = "💰 " + _today_money.ToString() + "€";
            today_sellNb.Text = "🤝 " + _today_sellNb.ToString() +  " Ventes";
            if (_today_trend < 0)
            {
                today_trend.Text = "📉 " + _today_trend.ToString() + "%";
            }
            else
            {
                today_trend.Text = "📈 +" + _today_trend.ToString() + "%";
            }

            week_money.Text = "💰 " + _week_money.ToString() + "€";
            week_sellNb.Text = "🤝 " + _week_sellNb.ToString() + " Ventes";
            if (_week_trend < 0)
            {
                week_trend.Text = "📉 " + _week_trend.ToString() + "%";
            }
            else
            {
                week_trend.Text = "📈 +" + _week_trend.ToString() + "%";
            }

            average_money.Text = "💰 " + _average_money.ToString() + "€";
            average_sellNb.Text = "⚙️ " + _average_sellNb.ToString() + " Articles";
            if(_average_trend < 0)
            {
                average_trend.Text = "📉 " + _average_trend.ToString() + "%";
            }
            else
            {
                average_trend.Text = "📈 +" + _average_trend.ToString() + "%";
            }

            bestCustomer_money.Text = "💰 " + _bestCustomer_money + "€";
            bestCustomer_name.Text = "👨‍💼 " + _bestCustomer_name;
            bestCustomer_city.Text = "🏠 " + _bestCustomer_city;
        }

        public async void ComputeStats()
        {
            MySqlConnection maConnexion;
            try
            {
                int average_sellNb = 0;
                int average_money = 0;
                int i = 0;

                double yesterday_money = 0;
                double last_week_money = 0;

                DateTime today = DateTime.Now.Date;

                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT montant, date_commande from commandes;";
                MySqlCommand command = maConnexion.CreateCommand();
                command.CommandText = requete;

                var reader = command.ExecuteReader();

                string[] valueString = new string[reader.FieldCount];
                while (reader.Read())
                {
                    if ( (DateTime)reader["date_commande"] == today)
                    {
                        _today_money += Convert.ToInt32((double)reader["montant"]);
                        _today_sellNb++;
                    }

                    if ((DateTime)reader["date_commande"] == today.AddDays(-1))
                    {
                        yesterday_money += Convert.ToInt32((double)reader["montant"]);
                    }


                    if ((DateTime)reader["date_commande"] >= today.AddDays(-7))
                    {
                        _week_money += Convert.ToInt32((double)reader["montant"]);
                        _week_sellNb++;
                    }

                    if ((DateTime)reader["date_commande"] >= today.AddDays(-14) && (DateTime)reader["date_commande"] < today.AddDays(-7))
                    {
                        last_week_money += Convert.ToInt32((double)reader["montant"]);
                    }

                    average_money +=Convert.ToInt32((double)reader["montant"]);
                    i++;
                }

                _average_money = average_money / i;

                _today_trend = Math.Round(((double)_today_money - yesterday_money) / yesterday_money * 100, 2);
                //Console.WriteLine("t trend");
                //Console.WriteLine(_today_trend);

                _week_trend = Math.Round(((double)_week_money - last_week_money) / last_week_money * 100, 2);
                //Console.WriteLine("w trend");
                //Console.WriteLine(_week_trend);

                reader.Close();
                command.Dispose();
                maConnexion.Close();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }

            try
            {
                long i = 0;
                long nbSellByCustomer = 0;

                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT COUNT(commandes.id_client) AS id FROM pieces_commandes INNER JOIN commandes ON commandes.id = pieces_commandes.commande_id GROUP BY commandes.id_client;";
                MySqlCommand command = maConnexion.CreateCommand();
                command.CommandText = requete;

                var reader = command.ExecuteReader();

                string[] valueString = new string[reader.FieldCount];
                while (reader.Read())
                {
                    //Console.WriteLine((long)reader["id"]);
                    nbSellByCustomer += (long)reader["id"];
                    i++;
                }

                _average_sellNb = Convert.ToInt32(nbSellByCustomer / i);

                reader.Close();
                command.Dispose();
                maConnexion.Close();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }

            try
            {
                double maxAmount = 0;
                string prenom = "";
                string ville = "";

                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT SUM(commandes.montant) AS montant, individus.ville AS ville, individus.prenom AS prenom FROM commandes INNER JOIN individus ON individus.nom = commandes.id_client ;";
                MySqlCommand command = maConnexion.CreateCommand();
                command.CommandText = requete;

                var reader = command.ExecuteReader();

                string[] valueString = new string[reader.FieldCount];
                while (reader.Read())
                {
                    maxAmount += (double)reader["montant"];
                    prenom = (string)reader["prenom"];
                    ville = (string)reader["ville"];
                }

                _bestCustomer_money = Convert.ToInt32(maxAmount);
                _bestCustomer_name = prenom;
                _bestCustomer_city = ville;

                reader.Close();
                command.Dispose();
                maConnexion.Close();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }
        }

        private void openUrl(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.alltrails.com/fr/randonnee/france/hautes-pyrenees/pic-du-midi-de-bigorre-et-lac-d-oncet",
                UseShellExecute = true
            });
        }

        private void openUrl2(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.google.fr/maps/place/Pic+du+Midi+de+Bigorre/@42.9366024,0.1307788,4615m/data=!3m1!1e3!4m8!1m2!2m1!1sPic+du+Midi+de+Bigorre+et+Lac+d'Oncet+difficile+%C3%89toile+jaune+%C3%89toile+jaune+%C3%89toile+jaune+%C3%89toile+jaune+%C3%89toile+jaune+%C3%89toile+grise+(97)+Parc+national+des+Pyr%C3%A9n%C3%A9es!3m4!1s0x12a8235749919997:0x136b76e1ff9b200!8m2!3d42.9368725!4d0.1410922",
                UseShellExecute = true
            });
        }
    }
}

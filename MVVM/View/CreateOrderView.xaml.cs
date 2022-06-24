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
using System.Threading.Tasks;
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
    public partial class CreateOrderView : UserControl
    {
        public static string bicyclette_nom;
        public static string bicyclette_grandeur;

        public static int highestId;
        public static float prixTotal;

        public static string client;
        public static string client_type;

        public CreateOrderView()
        {
            InitializeComponent();

            loadSQLData();

            /*string MODEL_PATH = "\\bike.obj";
            ModelVisual3D device3D;

            device3D = new ModelVisual3D();
            device3D.Content = Display3d(MODEL_PATH);

            viewPort3d.Children.Add(device3D);*/
        }

        /*private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                viewPort3d.RotateGesture = new MouseGesture(MouseAction.LeftClick);


                ModelImporter import = new ModelImporter();


                device = import.Load(Environment.CurrentDirectory + model);
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }*/

        private async void CreateOrder(object sender, RoutedEventArgs e)
        {
            if (comboBox_1.SelectedItem != null && comboBox_2.SelectedItem != null && comboBox_3.SelectedItem != null && comboBox_4.SelectedItem != null) {

                try
                {
                    ///////////////////  INFO CLIENT //////////////////////

                    string rue = "";
                    string code_postal = "";
                    string ville = "";
                    string province = "";

                    string Query = "";

                    if (client_type == "Individus")
                    {
                        // Exemple 1 Requête avec auto-jointure LEFT JOIN
                        Query = "SELECT individus.rue, individus.code_postal, individus.ville, individus.province FROM individus LEFT JOIN individus as i ON individus.rue = i.rue WHERE i.nom = '" + client + "';";
                    }
                    else
                    {
                        // Exemple 1 Requête avec une union UNION ALL
                        Query = "SELECT rue,code_postal,ville,province FROM individus UNION ALL SELECT rue,code_postal,ville,province FROM boutiques_specialisees WHERE nom = '" + client + "';";
                    }

                    MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);

                    MyConn.Open();

                    var reader = await MyCommand.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        rue = (string)reader["rue"];
                        code_postal = (string)reader["code_postal"];
                        ville = (string)reader["ville"];
                        province = (string)reader["province"];
                    }

                    MyConn.Close();
                    MyCommand.Dispose();

                    //await Task.Delay(100);

                    ///////////////////  REFERENCES DES PIECES DU VELO //////////////////////
                   
                    //Console.WriteLine(bicylette_nom);
                    //Console.WriteLine(bicylette_grandeur);

                    string Query3 = "SELECT cadre, guidon, freins, selle, derailleur_avant, derailleur_arriere, roue_avant, roue_arriere, reflecteurs, pedalier, ordinateur, panier FROM assemblages WHERE nom = '" + bicyclette_nom + "' AND grandeur = '" + bicyclette_grandeur + "';";

                    MySqlConnection MyConn3 = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand3 = new MySqlCommand(Query3, MyConn3);

                    MyConn3.Open();

                    string _Cadre = "";
                    string _Guidon = "";
                    string _Freins = "";
                    string _Selle = "";
                    string _Derailleur_avant = "";
                    string _Derailleur_arriere = "";
                    string _Roue_avant = "";
                    string _Roue_arriere = "";
                    string _Reflecteurs = "";
                    string _Pedalier = "";
                    string _Ordinateur = "";
                    string _Panier = "";

                    var reader3 = await MyCommand3.ExecuteReaderAsync();
                    while (await reader3.ReadAsync())
                    {
                        _Cadre = (string)reader3["cadre"];
                        _Guidon = (string)reader3["guidon"];
                        _Freins = (string)reader3["freins"];
                        _Selle = (string)reader3["selle"];
                        _Derailleur_avant = (string)reader3["derailleur_avant"];
                        _Derailleur_arriere = (string)reader3["derailleur_arriere"];
                        _Roue_avant = (string)reader3["roue_avant"];
                        _Roue_arriere = (string)reader3["roue_arriere"];
                        _Reflecteurs = (string)reader3["reflecteurs"];
                        _Pedalier = (string)reader3["pedalier"];
                        _Ordinateur = (string)reader3["ordinateur"];
                        _Panier = (string)reader3["panier"];
                    }

                    MyConn3.Close();
                    MyCommand3.Dispose();

                    //await Task.Delay(100);

                    ///////////////////  VERIFIER QUANTITE CHAQUE PIECE //////////////////////

                    if (!string.IsNullOrWhiteSpace(_Cadre))
                    {
                        try
                        {
                            string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Cadre + "';";

                            MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                            MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                            MyConn10.Open();

                            int qt = 0;
                            float prix = 0;

                            var reader10 = await MyCommand10.ExecuteReaderAsync();
                            while (await reader10.ReadAsync())
                            {
                                qt = (int)reader10["quantite"];
                                prix = (float)reader10["prix"];
                            }

                            MyConn10.Close();
                            MyCommand10.Dispose();

                            //Console.WriteLine(_Cadre.ToString());
                            //Console.WriteLine(qt.ToString());

                            if (qt > 0)
                            {
                                string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Cadre + "';";
                                MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                MyConn11.Open();
                                await MyCommand11.ExecuteReaderAsync();
                                MyConn11.Close();
                            }
                            else
                            {
                                MessageBox.Show("Quantité de " + _Cadre + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(_Guidon))
                    {
                        try
                        {
                            string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Guidon + "';";

                            MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                            MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                            MyConn10.Open();

                            int qt = 0;
                            float prix = 0;

                            var reader10 = await MyCommand10.ExecuteReaderAsync();
                            while (await reader10.ReadAsync())
                            {
                                qt = (int)reader10["quantite"];
                                prix = (float)reader10["prix"];
                            }

                            MyConn10.Close();
                            MyCommand10.Dispose();

                            //Console.WriteLine(_Cadre.ToString());
                            //Console.WriteLine(qt.ToString());

                            if (qt > 0)
                            {
                                string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Guidon + "';";
                                MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                MyConn11.Open();
                                await MyCommand11.ExecuteReaderAsync();
                                MyConn11.Close();
                            }
                            else
                            {
                                MessageBox.Show("Quantité de " + _Guidon + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(_Freins))
                    {
                        try
                        {
                            string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Freins + "';";

                            MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                            MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                            MyConn10.Open();

                            int qt = 0;
                            float prix = 0;

                            var reader10 = await MyCommand10.ExecuteReaderAsync();
                            while (await reader10.ReadAsync())
                            {
                                qt = (int)reader10["quantite"];
                                prix = (float)reader10["prix"];
                            }

                            MyConn10.Close();
                            MyCommand10.Dispose();

                            //Console.WriteLine(_Freins.ToString());
                            //Console.WriteLine(qt.ToString());

                            if (qt > 0)
                            {
                                string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Freins + "';";
                                MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                MyConn11.Open();
                                await MyCommand11.ExecuteReaderAsync();
                                MyConn11.Close();
                            }
                            else
                            {
                                MessageBox.Show("Quantité de " + _Freins + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(_Selle))
                    {
                        try
                        {
                            string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Selle + "';";

                            MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                            MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                            MyConn10.Open();

                            int qt = 0;
                            float prix = 0;

                            var reader10 = await MyCommand10.ExecuteReaderAsync();
                            while (await reader10.ReadAsync())
                            {
                                qt = (int)reader10["quantite"];
                                prix = (float)reader10["prix"];
                            }

                            MyConn10.Close();
                            MyCommand10.Dispose();

                            //Console.WriteLine(_Selle.ToString());
                            //Console.WriteLine(qt.ToString());

                            if (qt > 0)
                            {
                                string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Selle + "';";
                                MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                MyConn11.Open();
                                await MyCommand11.ExecuteReaderAsync();
                                MyConn11.Close();
                            }
                            else
                            {
                                MessageBox.Show("Quantité de " + _Selle + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        if (!string.IsNullOrWhiteSpace(_Derailleur_avant))
                        {
                            try
                            {
                                string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Derailleur_avant + "';";

                                MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                                MyConn10.Open();

                                int qt = 0;
                                float prix = 0;

                                var reader10 = await MyCommand10.ExecuteReaderAsync();
                                while (await reader10.ReadAsync())
                                {
                                    qt = (int)reader10["quantite"];
                                    prix = (float)reader10["prix"];
                                }

                                MyConn10.Close();
                                MyCommand10.Dispose();

                                //Console.WriteLine(_Derailleur_avant.ToString());
                                //Console.WriteLine(qt.ToString());

                                if (qt > 0)
                                {
                                    string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Derailleur_avant + "';";
                                    MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                    MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                    MyConn11.Open();
                                    await MyCommand11.ExecuteReaderAsync();
                                    MyConn11.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Quantité de " + _Derailleur_avant + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(_Derailleur_arriere))
                        {
                            try
                            {
                                string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Derailleur_arriere + "';";

                                MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                                MyConn10.Open();

                                int qt = 0;
                                float prix = 0;

                                var reader10 = await MyCommand10.ExecuteReaderAsync();
                                while (await reader10.ReadAsync())
                                {
                                    qt = (int)reader10["quantite"];
                                    prix = (float)reader10["prix"];
                                }

                                MyConn10.Close();
                                MyCommand10.Dispose();

                                //Console.WriteLine(_Derailleur_arriere.ToString());
                                //Console.WriteLine(qt.ToString());

                                if (qt > 0)
                                {
                                    string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Derailleur_arriere + "';";
                                    MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                    MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                    MyConn11.Open();
                                    await MyCommand11.ExecuteReaderAsync();
                                    MyConn11.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Quantité de " + _Derailleur_arriere + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(_Roue_avant))
                        {
                            try
                            {
                                string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Roue_avant + "';";

                                MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                                MyConn10.Open();

                                int qt = 0;
                                float prix = 0;

                                var reader10 = await MyCommand10.ExecuteReaderAsync();
                                while (await reader10.ReadAsync())
                                {
                                    qt = (int)reader10["quantite"];
                                    prix = (float)reader10["prix"];
                                }

                                MyConn10.Close();
                                MyCommand10.Dispose();

                                //Console.WriteLine(_Derailleur_arriere.ToString());
                                //Console.WriteLine(qt.ToString());

                                if (qt > 0)
                                {
                                    string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Roue_avant + "';";
                                    MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                    MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                    MyConn11.Open();
                                    await MyCommand11.ExecuteReaderAsync();
                                    MyConn11.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Quantité de " + _Roue_avant + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(_Roue_arriere))
                        {
                            try
                            {
                                string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Roue_arriere + "';";

                                MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                                MyConn10.Open();

                                int qt = 0;
                                float prix = 0;

                                var reader10 = await MyCommand10.ExecuteReaderAsync();
                                while (await reader10.ReadAsync())
                                {
                                    qt = (int)reader10["quantite"];
                                    prix = (float)reader10["prix"];
                                }

                                MyConn10.Close();
                                MyCommand10.Dispose();

                                //Console.WriteLine(_Derailleur_arriere.ToString());
                                //Console.WriteLine(qt.ToString());

                                if (qt > 0)
                                {
                                    string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Roue_arriere + "';";
                                    MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                    MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                    MyConn11.Open();
                                    await MyCommand11.ExecuteReaderAsync();
                                    MyConn11.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Quantité de " + _Roue_arriere + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(_Reflecteurs))
                        {
                            try
                            {
                                string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Reflecteurs + "';";

                                MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                                MyConn10.Open();

                                int qt = 0;
                                float prix = 0;

                                var reader10 = await MyCommand10.ExecuteReaderAsync();
                                while (await reader10.ReadAsync())
                                {
                                    qt = (int)reader10["quantite"];
                                    prix = (float)reader10["prix"];
                                }

                                MyConn10.Close();
                                MyCommand10.Dispose();

                                //Console.WriteLine(_Reflecteurs.ToString());
                                //Console.WriteLine(qt.ToString());

                                if (qt > 0)
                                {
                                    string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Reflecteurs + "';";
                                    MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                    MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                    MyConn11.Open();
                                    await MyCommand11.ExecuteReaderAsync();
                                    MyConn11.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Quantité de " + _Reflecteurs + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(_Pedalier))
                        {
                            try
                            {
                                string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Pedalier + "';";

                                MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                                MyConn10.Open();

                                int qt = 0;
                                float prix = 0;

                                var reader10 = await MyCommand10.ExecuteReaderAsync();
                                while (await reader10.ReadAsync())
                                {
                                    qt = (int)reader10["quantite"];
                                    prix = (float)reader10["prix"];
                                }

                                MyConn10.Close();
                                MyCommand10.Dispose();

                                //Console.WriteLine(_Pedalier.ToString());
                               // Console.WriteLine(qt.ToString());

                                if (qt > 0)
                                {
                                    string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Pedalier + "';";
                                    MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                    MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                    MyConn11.Open();
                                    await MyCommand11.ExecuteReaderAsync();
                                    MyConn11.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Quantité de " + _Pedalier + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(_Ordinateur))
                        {
                            try
                            {
                                string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Ordinateur + "';";

                                MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                                MyConn10.Open();

                                int qt = 0;
                                float prix = 0;

                                var reader10 = await MyCommand10.ExecuteReaderAsync();
                                while (await reader10.ReadAsync())
                                {
                                    qt = (int)reader10["quantite"];
                                    prix = (float)reader10["prix"];
                                }

                                MyConn10.Close();
                                MyCommand10.Dispose();

                                //Console.WriteLine(_Ordinateur.ToString());
                                //Console.WriteLine(qt.ToString());

                                if (qt > 0)
                                {
                                    string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Ordinateur + "';";
                                    MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                    MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                    MyConn11.Open();
                                    await MyCommand11.ExecuteReaderAsync();
                                    MyConn11.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Quantité de " + _Ordinateur + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(_Panier))
                        {
                            try
                            {
                                string Query10 = "SELECT quantite, prix FROM pieces WHERE numero_produit = '" + _Panier + "';";

                                MySqlConnection MyConn10 = new MySqlConnection(Variables_Manager.adminLogin);
                                MySqlCommand MyCommand10 = new MySqlCommand(Query10, MyConn10);

                                MyConn10.Open();

                                int qt = 0;
                                float prix = 0;

                                var reader10 = await MyCommand10.ExecuteReaderAsync();
                                while (await reader10.ReadAsync())
                                {
                                    qt = (int)reader10["quantite"];
                                    prix = (float)reader10["prix"];
                                }

                                MyConn10.Close();
                                MyCommand10.Dispose();

                                //Console.WriteLine(_Panier.ToString());
                                //Console.WriteLine(qt.ToString());

                                if (qt > 0)
                                {
                                    string Query11 = "UPDATE pieces SET quantite = quantite - 1 WHERE numero_produit = '" + _Panier + "';";
                                    MySqlConnection MyConn11 = new MySqlConnection(Variables_Manager.adminLogin);
                                    MySqlCommand MyCommand11 = new MySqlCommand(Query11, MyConn11);
                                    MyConn11.Open();
                                    await MyCommand11.ExecuteReaderAsync();
                                    MyConn11.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Quantité de " + _Panier + " insuffisante. Veuillez en rajouter en stock.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }

                    // await Task.Delay(100);

                    //////////// SELECTIONNER L'ID LA PLUS ELEVE DANS AL LISTE DE COMMANDES POUR L'INCREMENTER DE 1

                    string Query5 = "SELECT id FROM commandes;";

                    MySqlConnection MyConn5 = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand5 = new MySqlCommand(Query5, MyConn5);

                    MyConn5.Open();

                    var reader5 = await MyCommand5.ExecuteReaderAsync();
                    while (await reader5.ReadAsync())
                    {
                        string highestId_str = (string)reader5["id"];
                        int _highestId = Convert.ToInt32(highestId_str.Remove(0, 1));
                        if(_highestId > highestId)
                        {
                            highestId = _highestId;
                        }
                    }

                    highestId++;

                    MyConn5.Close();
                    MyCommand5.Dispose();

                    ///////////////////  AJOUTER PRODUITS DANS UNE TABLE SPECIAL pieces_commandes RELIERS A LA TABLE prouits ET commandes //////////////////////

                    string Query4 = "INSERT INTO pieces_commandes (commande_id, piece_id) VALUES ('C" + highestId.ToString() + "','" + bicyclette_nom + "');";

                    MySqlConnection MyConn4 = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand4 = new MySqlCommand(Query4, MyConn4);

                    MyConn4.Open();
                    await MyCommand4.ExecuteReaderAsync();

                    MyConn4.Close();
                    MyCommand4.Dispose();

                    ///////////////////  AJOUT LIGNE //////////////////////

                    string today = DateTime.Now.ToString("yyyy-MM-dd");
                    string Query2 = "INSERT INTO commandes (id, id_client, date_commande, rue, code_postal, ville, province, date_livraison, montant) VALUES ('C" + highestId.ToString() + "','" + client + "','" + today + "','" + rue + "','" + code_postal + "','" + ville + "','" + province + "','" + today +"','" + prixTotal.ToString() +  "');";

                    MySqlConnection MyConn2 = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand2 = new MySqlCommand(Query2, MyConn2);

                    MyConn2.Open();
                    await MyCommand2.ExecuteReaderAsync();

                    MyConn2.Close();
                    MyCommand2.Dispose();

                    ///////////////////  AJOUTER ARGENT DE LA VENTE AU COMPTE //////////////////////

                    Variables_Manager.money += Math.Round(prixTotal);
                    App.Current.Dispatcher.Invoke(delegate {
                        MainWindow.moneyTextStatic.Text = "Compte: " + Variables_Manager.money.ToString() + ",00€";
                    });

                    string _Query = "UPDATE manager SET argent = '" + Variables_Manager.money + "';";
                    MySqlConnection _MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand _MyCommand = new MySqlCommand(_Query, _MyConn);
                    _MyConn.Open();
                    _MyCommand.ExecuteReader();
                    _MyConn.Close();

                    highestId++;


                    MessageBox.Show("Commande passée !", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Certains champs sont vides.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void loadSQLData()
        {

            MySqlConnection maConnexion;
            try
            {
                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT nom from bicyclettes;";
                MySqlCommand command1 = maConnexion.CreateCommand();
                command1.CommandText = requete;

                var reader = await command1.ExecuteReaderAsync();

                string[] valueString = new string[reader.FieldCount];
                while (await reader.ReadAsync())
                {
                    string nom = (string)reader["nom"];
                    comboBox_1.Items.Add(nom);
                    //prixTotal = (float)reader["prix"];
                }

                await reader.CloseAsync();
                await command1.DisposeAsync();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }
        }

        private async void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            string _s = cmb.SelectedItem.ToString();
            if (_s.Length > 0)
            {
                bicyclette_nom = _s;

                MySqlConnection maConnexion;
                try
                {
                    maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                    maConnexion.Open();

                    comboBox_2.Items.Clear();

                    string requete = "SELECT grandeur from bicyclettes WHERE nom = '" + comboBox_1.SelectedItem.ToString() + "';";
                    MySqlCommand command1 = maConnexion.CreateCommand();
                    command1.CommandText = requete;

                    var reader = await command1.ExecuteReaderAsync();

                    string[] valueString = new string[reader.FieldCount];
                    while (await reader.ReadAsync())
                    {
                        string grandeur = (string)reader["grandeur"];
                        comboBox_2.Items.Add(grandeur);
                    }

                    //comboBox_2.SelectedItem = comboBox_2.Items[0];
                    comboBox_2.Visibility = Visibility.Visible;

                    await reader.CloseAsync();
                    await command1.DisposeAsync();
                }
                catch (MySqlException e1)
                {
                    Console.Write("Erreur de Connexion : " + e1.ToString());
                }
            }
        }

        private async void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            string _s = "";
            if (cmb.SelectedItem != null)
            {
                _s = cmb.SelectedItem.ToString();
            }
            if (_s.Length > 0)
            {
                bicyclette_grandeur = _s;

                comboBox_3.Visibility = Visibility.Visible;
                clientText.Visibility = Visibility.Visible;

                MySqlConnection maConnexion;
                try
                {
                    maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                    maConnexion.Open();

                    string requete = "SELECT prix FROM bicyclettes WHERE grandeur = '" + bicyclette_grandeur + "' AND nom = '" + bicyclette_nom + "';";
                    MySqlCommand command1 = maConnexion.CreateCommand();
                    command1.CommandText = requete;

                    var reader = await command1.ExecuteReaderAsync();

                    string[] valueString = new string[reader.FieldCount];
                    while (await reader.ReadAsync())
                    {
                        prixTotal = (float)reader["prix"];
                    }

                    CreateOrderBtn.Content = "Ajouter " + prixTotal.ToString() + "Є";

                    await reader.CloseAsync();
                    await command1.DisposeAsync();
                }
                catch (MySqlException e1)
                {
                    Console.Write("Erreur de Connexion : " + e1.ToString());
                }
            }
        }

          private async void ComboBox_SelectionChanged_3(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            string _s = "";
            if (cmb.SelectedItem != null)
            {
                _s = cmb.SelectedItem.ToString();
            }
            if (_s.Length > 0) {

                string s = _s.Remove(0, 39);
                client_type = s;

                if (s == "Individus")
                {
                    MySqlConnection maConnexion;
                    try
                    {
                        maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                        maConnexion.Open();

                        comboBox_4.Items.Clear();

                        string requete = "SELECT nom FROM individus;";
                        MySqlCommand command1 = maConnexion.CreateCommand();
                        command1.CommandText = requete;

                        var reader = await command1.ExecuteReaderAsync();

                        string[] valueString = new string[reader.FieldCount];
                        while (await reader.ReadAsync())
                        {
                            string nom = (string)reader["nom"];
                            comboBox_4.Items.Add(nom);
                        }

                        //comboBox_3.SelectedItem = comboBox_3.Items[0];
                        comboBox_4.Visibility = Visibility.Visible;

                        await reader.CloseAsync();
                        await command1.DisposeAsync();
                    }
                    catch (MySqlException e1)
                    {
                        Console.Write("Erreur de Connexion : " + e1.ToString());
                    }
                }
                else
                {
                    MySqlConnection maConnexion;
                    try
                    {
                        maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                        maConnexion.Open();

                        comboBox_4.Items.Clear();

                        string requete = "SELECT nom FROM boutiques_specialisees;";
                        MySqlCommand command1 = maConnexion.CreateCommand();
                        command1.CommandText = requete;

                        var reader = await command1.ExecuteReaderAsync();

                        string[] valueString = new string[reader.FieldCount];
                        while (await reader.ReadAsync())
                        {
                            string nom = (string)reader["nom"];
                            comboBox_4.Items.Add(nom);
                        }

                        //comboBox_3.SelectedItem = comboBox_3.Items[0];
                        comboBox_4.Visibility = Visibility.Visible;
                        CreateOrderBtn.Visibility = Visibility.Visible;

                        await reader.CloseAsync();
                        await command1.DisposeAsync();
                    }
                    catch (MySqlException e1)
                    {
                        Console.Write("Erreur de Connexion : " + e1.ToString());
                    }
                }
            }
        }

        private async void ComboBox_SelectionChanged_4(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            string _s = "";
            if (cmb.SelectedItem != null)
            {
                _s = cmb.SelectedItem.ToString();
                client = _s;

                CreateOrderBtn.Visibility = Visibility.Visible;
            }
        }
    }
}

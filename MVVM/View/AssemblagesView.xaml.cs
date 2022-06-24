using HelixToolkit.Wpf;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
    public partial class AssemblagesView : UserControl
    {
        public static List<Assemblage> assemblages = new List<Assemblage>();

        public static int HighestNumber;

        public static string currentCellValue;
        public static string currentCellHeader;
        public static int currentCellID; // id


        public AssemblagesView()
        {
            InitializeComponent();

            dataGrid.ItemsSource = Assemblage.GetAssemblages();

            dataGrid.AutoGeneratingColumn += DataGrid_UpdateSource;
            dataGrid.GotFocus += DataGrid_GotFocus;
            dataGrid.PreparingCellForEdit += DataGrid_PreparingCellForEdit;
            dataGrid.CellEditEnding += DataGrid_CellEditEnding;
            dataGrid.AddingNewItem += DataGrid_AddNewRow;
            dataGrid.Sorting += DataGrid_Sorting;
        }

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ?
                                ListSortDirection.Ascending : ListSortDirection.Descending;

            //MessageBox.Show(e.Column.Header.ToString());

            if (direction == ListSortDirection.Ascending)
            {
                if (e.Column.Header.ToString() == "Nom")
                {
                    assemblages = assemblages.OrderBy(i => i.Nom).ToList();
                }
                else if (e.Column.Header.ToString() == "Grandeur")
                {
                    assemblages = assemblages.OrderBy(i => i.Grandeur).ToList();
                }
                else if (e.Column.Header.ToString() == "Cadre")
                {
                    assemblages = assemblages.OrderBy(i => i.Cadre).ToList();
                }
                else if (e.Column.Header.ToString() == "Guidon")
                {
                    assemblages = assemblages.OrderBy(i => i.Guidon).ToList();
                }
                else if (e.Column.Header.ToString() == "Freins")
                {
                    assemblages = assemblages.OrderBy(i => i.Freins).ToList();
                }
                else if (e.Column.Header.ToString() == "Selle")
                {
                    assemblages = assemblages.OrderBy(i => i.Selle).ToList();
                }
                else if (e.Column.Header.ToString() == "Derailleur avant")
                {
                    assemblages = assemblages.OrderBy(i => i.Derailleur_avant).ToList();
                }
                else if (e.Column.Header.ToString() == "Derailleur arriere")
                {
                    assemblages = assemblages.OrderBy(i => i.Derailleur_arriere).ToList();
                }
                else if (e.Column.Header.ToString() == "Roue avant")
                {
                    assemblages = assemblages.OrderBy(i => i.Roue_avant).ToList();
                }
                else if (e.Column.Header.ToString() == "Roue arriere")
                {
                    assemblages = assemblages.OrderBy(i => i.Roue_arriere).ToList();
                }
                else if (e.Column.Header.ToString() == "Refelcteurs")
                {
                    assemblages = assemblages.OrderBy(i => i.Reflecteurs).ToList();
                }
                else if (e.Column.Header.ToString() == "Pedalier")
                {
                    assemblages = assemblages.OrderBy(i => i.Pedalier).ToList();
                }
                else if (e.Column.Header.ToString() == "Ordinateur")
                {
                    assemblages = assemblages.OrderBy(i => i.Ordinateur).ToList();
                }
                else if (e.Column.Header.ToString() == "Panier")
                {
                    assemblages = assemblages.OrderBy(i => i.Panier).ToList();
                }
            }
            else
            {
                if (e.Column.Header.ToString() == "Nom")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Nom).ToList();
                }
                else if (e.Column.Header.ToString() == "Grandeur")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Grandeur).ToList();
                }
                else if (e.Column.Header.ToString() == "Cadre")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Cadre).ToList();
                }
                else if (e.Column.Header.ToString() == "Guidon")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Guidon).ToList();
                }
                else if (e.Column.Header.ToString() == "Freins")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Freins).ToList();
                }
                else if (e.Column.Header.ToString() == "Selle")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Selle).ToList();
                }
                else if (e.Column.Header.ToString() == "Derailleur avant")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Derailleur_avant).ToList();
                }
                else if (e.Column.Header.ToString() == "Derailleur arriere")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Derailleur_arriere).ToList();
                }
                else if (e.Column.Header.ToString() == "Roue avant")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Roue_avant).ToList();
                }
                else if (e.Column.Header.ToString() == "Roue arriere")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Roue_arriere).ToList();
                }
                else if (e.Column.Header.ToString() == "Refelcteurs")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Reflecteurs).ToList();
                }
                else if (e.Column.Header.ToString() == "Pedalier")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Pedalier).ToList();
                }
                else if (e.Column.Header.ToString() == "Ordinateur")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Ordinateur).ToList();
                }
                else if (e.Column.Header.ToString() == "Panier")
                {
                    assemblages = assemblages.OrderByDescending(i => i.Panier).ToList();
                }
            }
            
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var uiElement = e.OriginalSource as UIElement;
                if (e.Key == Key.Enter && uiElement != null)
                {
                    e.Handled = true;
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
                else if (e.Key == Key.Right && uiElement != null)
                {
                    e.Handled = true;
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                }
                else if (e.Key == Key.Left && uiElement != null)
                {
                    e.Handled = true;
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
                }
                else if (e.Key == Key.Up && uiElement != null)
                {
                    e.Handled = true;
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                }
                else if (e.Key == Key.Down && uiElement != null)
                {
                    e.Handled = true;
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                }
            }
            catch (Exception e1)
            {

            }
        }

        async void DataGrid_GotFocus(object sender, RoutedEventArgs e) // problem add
        {
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                try
                {
                    DataGrid grd = (DataGrid)sender;
                    await Task.Delay(100);
                    grd.BeginEdit(e);
                }
                catch (Exception e1) { }
            }
        }

        void DataGrid_UpdateSource(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (dataGrid.Columns.Count > 0)
            {
                dataGrid.Columns[0].Visibility = Visibility.Hidden;
            }

            if (e.PropertyType == typeof(DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "yyyy-MM-dd";
            }

            string header = e.Column.Header.ToString();
            e.Column.Header = header.Replace("_", " ");
        }

        void DataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            try
            {
                var editingTextBox = e.EditingElement as TextBox;
                var newValue = editingTextBox.Text;

                currentCellValue = newValue;
                currentCellHeader = e.Column.Header.ToString();

                currentCellID = assemblages[e.Row.GetIndex()].Numero;

                //MessageBox.Show(currentCellID.ToString());
            }
            catch (Exception e1) { }
        }

        void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editingTextBox = e.EditingElement as TextBox;
            var newValue = editingTextBox.Text;

            try
            {
                string Query = "UPDATE assemblages SET " + e.Column.Header.ToString().Replace(" ", "_") + "='" + newValue + "' WHERE numero ='" + currentCellID + "';";
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

        async void DataGrid_AddNewRow(object sender, AddingNewItemEventArgs e)
        {
                try
                {
                    // Reset Sorting to avoid error
                    ICollectionView view = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
                    if (view != null)
                    {
                        view.SortDescriptions.Clear();
                        foreach (DataGridColumn column in dataGrid.Columns)
                        {
                            column.SortDirection = null;
                        }
                    assemblages = assemblages.OrderBy(i => i.Numero).ToList();
                    }


                    HighestNumber++;

                    string Query = "INSERT INTO assemblages (numero, nom, grandeur, cadre, guidon, freins, selle, derailleur_avant, derailleur_arriere, roue_avant, roue_arriere, reflecteurs, pedalier, ordinateur, panier) VALUES ('" + (HighestNumber).ToString() + "','','','','','','','','','','','','','','');";
                    MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                    MyConn.Open();
                    MyCommand2.ExecuteReader();
                    MyConn.Close();
                    MyCommand2.Dispose();

                    await Task.Delay(100);

                    assemblages.Clear();
                    dataGrid.ItemsSource =Assemblage.GetAssemblages();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }

    private async void DeleteRow(object sender, RoutedEventArgs e)
    {
        string message = "Êtes-vous sûre de vouloir supprimer cette ligne ?";
        string caption = "Confirmation";
        MessageBoxButton buttons = MessageBoxButton.YesNo;
        MessageBoxImage icon = MessageBoxImage.Question;
        if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
        {
            try
            {
                string Query = "DELETE FROM assemblages WHERE numero = '" + currentCellID + "';";
                MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                MyConn.Open();
                MyCommand2.ExecuteReader();
                MyConn.Close();
                MyCommand2.Dispose();

                await Task.Delay(100);

                assemblages.Clear();
                dataGrid.ItemsSource = Assemblage.GetAssemblages();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

        private void ExportFile(object sender, RoutedEventArgs e)
        {
            if (Variables_Manager.export_format == ".Json")
            {
                SaveAsJSON();
            }
            else if (Variables_Manager.export_format == ".Xml")
            {
                SaveAsXML();
            }
            else if (Variables_Manager.export_format == ".Csv")
            {
                SaveAsCSV();
            }
        }

        private void SaveAsXML()
        {
            try
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string path = fbd.SelectedPath + "/Assemblages.xml";

                        var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Assemblage>));

                        var file = new StreamWriter(path);
                        writer.Serialize(file, assemblages);
                        file.Close();

                        MessageBox.Show("Fichier XML enregistré avec succès !", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Vous n'avez pas selectionné de dossier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveAsJSON()
        {
            try
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string path = fbd.SelectedPath + "/Assemblages.json";

                        string jsonRepresentation = JsonConvert.SerializeObject(assemblages);
                        File.WriteAllText(path, jsonRepresentation);

                        MessageBox.Show("Fichier JSON enregistré avec succès !", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Vous n'avez pas selectionné de dossier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SaveAsCSV()
        {
            try
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string path = fbd.SelectedPath + "/Assemblages.csv";

                        var csvFile = new List<string>();
                        foreach (Assemblage assemblage in assemblages)
                        {
                            csvFile.Add(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}",
                                assemblage.Nom, assemblage.Grandeur, assemblage.Guidon,
                                assemblage.Freins, assemblage.Selle,assemblage.Derailleur_avant,
                                assemblage.Derailleur_arriere, assemblage.Roue_avant,
                                assemblage.Roue_arriere,  assemblage.Reflecteurs, assemblage.Pedalier,
                                assemblage.Ordinateur, assemblage.Panier
                                )
                            );
                        }
                        File.WriteAllLines(path, csvFile);

                        MessageBox.Show("Fichier CSV enregistré avec succès !", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Erreur : Vous n'avez pas selectionné de dossier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("Erreur : " + e1.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class Assemblage : INotifyPropertyChanged
    {
        public int Numero { get; set; }
        public string Nom { get; set; }
        public string Grandeur { get; set; }
        public string Cadre { get; set; }
        public string Guidon { get; set; }
        public string Freins { get; set; }
        public string Selle { get; set; }
        public string Derailleur_avant { get; set; }
        public string Derailleur_arriere { get; set; }
        public string Roue_avant { get; set; }
        public string Roue_arriere { get; set; }
        public string Reflecteurs { get; set; }
        public string Pedalier { get; set; }
        public string Ordinateur { get; set; }
        public string Panier { get; set; }

        public static ObservableCollection<Assemblage> GetAssemblages()
        {
            var assemblages = new ObservableCollection<Assemblage>();

            // Load SQL Data
            MySqlConnection maConnexion;
            try
            {
                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT * from assemblages;";
                MySqlCommand command1 = maConnexion.CreateCommand();
                command1.CommandText = requete;

                MySqlDataReader reader = command1.ExecuteReader();

                string[] valueString = new string[reader.FieldCount];
                while (reader.Read())
                {
                    int _Numero = (int)reader["numero"];
                    string _Nom = (string)reader["nom"];
                    string _Grandeur = (string)reader["grandeur"];
                    string _Cadre = (string)reader["cadre"];
                    string _Guidon = (string)reader["guidon"];
                    string _Freins = (string)reader["freins"];
                    string _Selle = (string)reader["selle"];
                    string _Derailleur_avant = (string)reader["derailleur_avant"];
                    string _Derailleur_arriere = (string)reader["derailleur_arriere"];
                    string _Roue_avant = (string)reader["roue_avant"];
                    string _Roue_arriere = (string)reader["roue_arriere"];
                    string _Reflecteurs = (string)reader["reflecteurs"];
                    string _Pedalier = (string)reader["pedalier"];
                    string _Ordinateur = (string)reader["ordinateur"];
                    string _Panier = (string)reader["panier"];

                    if(_Numero > AssemblagesView.HighestNumber)
                    {
                        AssemblagesView.HighestNumber = _Numero;
                    }

                    assemblages.Add(new Assemblage()
                    {
                        Numero = _Numero,
                        Nom = _Nom,
                        Grandeur = _Grandeur,
                        Cadre = _Cadre,
                        Guidon = _Guidon,
                        Freins = _Freins,
                        Selle = _Selle,
                        Derailleur_avant = _Derailleur_avant,
                        Derailleur_arriere = _Derailleur_arriere,
                        Roue_avant = _Roue_avant,
                        Roue_arriere = _Roue_arriere,
                        Reflecteurs = _Reflecteurs,
                        Pedalier = _Pedalier,
                        Ordinateur = _Ordinateur,
                        Panier = _Pedalier,
                    });

                    AssemblagesView.assemblages.Add(new Assemblage()
                    {
                        Numero = _Numero,
                        Nom = _Nom,
                        Grandeur = _Grandeur,
                        Cadre = _Cadre,
                        Guidon = _Guidon,
                        Freins = _Freins,
                        Selle = _Selle,
                        Derailleur_avant = _Derailleur_avant,
                        Derailleur_arriere = _Derailleur_arriere,
                        Roue_avant = _Roue_avant,
                        Roue_arriere = _Roue_arriere,
                        Reflecteurs = _Reflecteurs,
                        Pedalier = _Pedalier,
                        Ordinateur = _Ordinateur,
                        Panier = _Pedalier,
                    });
                }

                reader.Close();
                command1.Dispose();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }


            return assemblages;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaiseProperChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}

using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
    /// <summary>
    /// Logique d'interaction pour OrdersView.xaml
    /// </summary>
    public partial class ClientsView : UserControl
    {
        public static List<Individu> individus = new List<Individu>();
        public static List<Boutique> boutiques = new List<Boutique>();

        public static int individus_HighestNumber;
        public static int boutiques_HighestNumber;

        public static string currentCellValue;
        public static string currentCellHeader;
        public static int currentCellNumber; // id
        public static string currentTab;

        public ClientsView()
        {
            InitializeComponent();

            individusDataGrid.ItemsSource = Individu.GetIndividus();
            boutiquesDataGrid.ItemsSource = Boutique.GetBoutiques();

            individusDataGrid.AutoGeneratingColumn += DataGrid_UpdateSource;
            boutiquesDataGrid.AutoGeneratingColumn += DataGrid_UpdateSource;

            individusDataGrid.GotFocus += DataGrid_GotFocus;
            boutiquesDataGrid.GotFocus += DataGrid_GotFocus;

            individusDataGrid.PreparingCellForEdit += DataGrid_PreparingCellForEdit;
            boutiquesDataGrid.PreparingCellForEdit += DataGrid_PreparingCellForEdit;

            individusDataGrid.CellEditEnding += DataGrid_CellEditEnding;
            boutiquesDataGrid.CellEditEnding += DataGrid_CellEditEnding;

            individusDataGrid.AddingNewItem += DataGrid_AddNewRow;
            boutiquesDataGrid.AddingNewItem += DataGrid_AddNewRow;

            individusDataGrid.Sorting += DataGrid_Sorting;
            boutiquesDataGrid.Sorting += DataGrid_Sorting;
        }

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ?
                                ListSortDirection.Ascending : ListSortDirection.Descending;

            if (currentTab == "individusDataGrid")
            {
                //MessageBox.Show(e.Column.Header.ToString());

                if (direction == ListSortDirection.Ascending)
                {
                    if (e.Column.Header.ToString() == "Nom")
                    {
                        individus = individus.OrderBy(i => i.Nom).ToList();
                    }
                    else if(e.Column.Header.ToString() == "Prenom")
                    {
                        individus = individus.OrderBy(i => i.Prenom).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Rue")
                    {
                        individus = individus.OrderBy(i => i.Rue).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Code postal")
                    {
                        individus = individus.OrderBy(i => i.Code_postal).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Ville")
                    {
                        individus = individus.OrderBy(i => i.Ville).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Province")
                    {
                        individus = individus.OrderBy(i => i.Province).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Programme")
                    {
                        individus = individus.OrderBy(i => i.Programme).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Date_debut_programme")
                    {
                        individus = individus.OrderBy(i => i.Date_debut_programme).ToList();
                    }
                }
                else
                {
                    if (e.Column.Header.ToString() == "Nom")
                    {
                        individus = individus.OrderByDescending(i => i.Nom).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Prenom")
                    {
                        individus = individus.OrderByDescending(i => i.Prenom).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Rue")
                    {
                        individus = individus.OrderByDescending(i => i.Rue).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Code postal")
                    {
                        individus = individus.OrderByDescending(i => i.Code_postal).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Ville")
                    {
                        individus = individus.OrderByDescending(i => i.Ville).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Province")
                    {
                        individus = individus.OrderByDescending(i => i.Province).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Programme")
                    {
                        individus = individus.OrderByDescending(i => i.Programme).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Date_debut_programme")
                    {
                        individus = individus.OrderByDescending(i => i.Date_debut_programme).ToList();
                    }
                }
            }

            else
            {
                if (direction == ListSortDirection.Ascending)
                {
                    if (e.Column.Header.ToString() == "Nom")
                    {
                        boutiques = boutiques.OrderBy(i => i.Nom).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Telephone")
                    {
                        boutiques = boutiques.OrderBy(i => i.Telephone).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Rue")
                    {
                        boutiques = boutiques.OrderBy(i => i.Rue).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Code postal")
                    {
                        boutiques = boutiques.OrderBy(i => i.Code_postal).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Ville")
                    {
                        boutiques = boutiques.OrderBy(i => i.Ville).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Province")
                    {
                        boutiques = boutiques.OrderBy(i => i.Province).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Courriel")
                    {
                        boutiques = boutiques.OrderBy(i => i.Courriel).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Nom cersonne contact")
                    {
                        boutiques = boutiques.OrderBy(i => i.Nom_personne_contact).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Pourcentage rabais")
                    {
                        boutiques = boutiques.OrderBy(i => i.Pourcentage_rabais).ToList();
                    }
                }
                else
                {
                    if (e.Column.Header.ToString() == "Nom")
                    {
                        boutiques = boutiques.OrderByDescending(i => i.Nom).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Telephone")
                    {
                        boutiques = boutiques.OrderByDescending(i => i.Telephone).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Rue")
                    {
                        boutiques = boutiques.OrderByDescending(i => i.Rue).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Code postal")
                    {
                        boutiques = boutiques.OrderByDescending(i => i.Code_postal).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Ville")
                    {
                        boutiques = boutiques.OrderByDescending(i => i.Ville).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Province")
                    {
                        boutiques = boutiques.OrderByDescending(i => i.Province).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Courriel")
                    {
                        boutiques = boutiques.OrderByDescending(i => i.Courriel).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Nom personne contact")
                    {
                        boutiques = boutiques.OrderByDescending(i => i.Nom_personne_contact).ToList();
                    }
                    else if (e.Column.Header.ToString() == "Pourcentage rabais")
                    {
                        boutiques = boutiques.OrderByDescending(i => i.Pourcentage_rabais).ToList();
                    }
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
            catch(Exception e1)
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

                    currentTab = grd.Name;

                    await Task.Delay(100);
                    grd.BeginEdit(e);
                }
                catch(Exception e1) { }
            }
        }

        void DataGrid_UpdateSource(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (individusDataGrid.Columns.Count > 0)
            {
                individusDataGrid.Columns[0].Visibility = Visibility.Hidden;
            }
            if (boutiquesDataGrid.Columns.Count > 0)
            {
                boutiquesDataGrid.Columns[0].Visibility = Visibility.Hidden;
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

                if (currentTab == "individusDataGrid")
                {
                    currentCellNumber = individus[e.Row.GetIndex()].Numero;
                }
                else
                {
                    currentCellNumber = boutiques[e.Row.GetIndex()].Numero;
                }

                //MessageBox.Show(currentCellNumber.ToString());
            }
            catch (Exception e1) { }
        }

        void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editingTextBox = e.EditingElement as TextBox;
            var newValue = editingTextBox.Text;

            if (currentTab == "individusDataGrid")
            {
                try
                {
                    string Query = "UPDATE individus SET " + e.Column.Header.ToString().Replace(" ", "_") + "='" + newValue + "' WHERE numero ='" + currentCellNumber + "';";
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
            else
            {
                try
                {
                    string Query = "UPDATE boutiques_specialisees SET " + e.Column.Header.ToString().Replace(" ", "_") + "='" + newValue + "' WHERE numero ='" + currentCellNumber + "';";
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

          async void DataGrid_AddNewRow(object sender, AddingNewItemEventArgs e)
          {
            if (currentTab == "individusDataGrid")
            {
                try
                {
                    // Reset Sorting to avoid error
                    ICollectionView view = CollectionViewSource.GetDefaultView(individusDataGrid.ItemsSource);
                    if (view != null)
                    {
                        view.SortDescriptions.Clear();
                        foreach (DataGridColumn column in individusDataGrid.Columns)
                        {
                            column.SortDirection = null;
                        }
                        individus = individus.OrderBy(i => i.Numero).ToList();
                    }


                    string today = DateTime.Now.ToString("yyyy-MM-dd");

                    individus_HighestNumber++;

                    string Query = "INSERT INTO individus (numero,nom,prenom,ville,rue,province,code_postal,programme,date_debut_programme) VALUES ('" + (individus_HighestNumber).ToString() + "','','','','','','','0','" + today + "');";
                    MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                    MyConn.Open();
                    MyCommand2.ExecuteReader();
                    MyConn.Close();
                    MyCommand2.Dispose();

                    await Task.Delay(100);

                    individus.Clear();
                    individusDataGrid.ItemsSource = Individu.GetIndividus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    // Reset Sorting to avoid error
                    ICollectionView view = CollectionViewSource.GetDefaultView(boutiquesDataGrid.ItemsSource);
                    if (view != null)
                    {
                        view.SortDescriptions.Clear();
                        foreach (DataGridColumn column in boutiquesDataGrid.Columns)
                        {
                            column.SortDirection = null;
                        }
                        boutiques = boutiques.OrderBy(i => i.Numero).ToList();
                    }


                    boutiques_HighestNumber++;

                    string Query = "INSERT INTO boutiques_specialisees (numero,nom,rue,code_postal,ville,province,telephone,courriel,nom_personne_contact,pourcentage_rabais) VALUES ('" + (boutiques_HighestNumber).ToString() + "','','','','','','','','','0');";
                    MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                    MyConn.Open();
                    MyCommand2.ExecuteReader();
                    MyConn.Close();
                    MyCommand2.Dispose();

                    await Task.Delay(100);

                    boutiques.Clear();
                    boutiquesDataGrid.ItemsSource = Boutique.GetBoutiques();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                if (currentTab == "individusDataGrid")
                {
                    try
                    {
                        string Query = "DELETE FROM individus WHERE numero = '" + currentCellNumber + "';";
                        MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                        MyConn.Open();
                        MyCommand2.ExecuteReader();
                        MyConn.Close();
                        MyCommand2.Dispose();

                        await Task.Delay(100);

                        individus.Clear();
                        individusDataGrid.ItemsSource = Individu.GetIndividus();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    try
                    {
                        string Query = "DELETE FROM boutiques_specialisees WHERE numero = '" + currentCellNumber + "';";
                        MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                        MyConn.Open();
                        MyCommand2.ExecuteReader();
                        MyConn.Close();
                        MyCommand2.Dispose();

                        await Task.Delay(100);

                        boutiques.Clear();
                        boutiquesDataGrid.ItemsSource = Boutique.GetBoutiques();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ExportFile(object sender, RoutedEventArgs e)
        {
            if(Variables_Manager.export_format == ".Json")
            {
                SaveAsJSON();
            }
            else if(Variables_Manager.export_format == ".Xml")
            {
                SaveAsXML();
            }
            else if(Variables_Manager.export_format == ".Csv")
            {
                SaveAsCSV();
            }
        }
        private async void SaveAsXML()
        {
            try
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string path = fbd.SelectedPath + "/Individus.xml";
                        string path2 = fbd.SelectedPath + "/Boutiques Spécialisées.xml";

                        var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Individu>));
                        var writer2 = new System.Xml.Serialization.XmlSerializer(typeof(List<Boutique>));

                        var file = new StreamWriter(path);
                        writer.Serialize(file, individus);
                        file.Close();

                        var file2 = new StreamWriter(path2);
                        writer2.Serialize(file2, boutiques);
                        file2.Close();

                        MessageBox.Show("Fichiers XML enregistrés avec succès !", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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
                        string path = fbd.SelectedPath + "/Individus.json";

                        string jsonRepresentation = JsonConvert.SerializeObject(individus);
                        File.WriteAllText(path, jsonRepresentation);


                        string path2 = fbd.SelectedPath + "/Boutiques Spécialisées.json";

                        string jsonRepresentation2 = JsonConvert.SerializeObject(boutiques);
                        File.WriteAllText(path2, jsonRepresentation2);

                        MessageBox.Show("Fichiers JSON enregistrés avec succès !", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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
                        string path = fbd.SelectedPath + "/Individus.csv";

                        var csvFile = new List<string>();
                        foreach (Individu individu in individus)
                        {
                            csvFile.Add(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                                individu.Nom, individu.Prenom,
                                individu.Ville, individu.Rue, individu.Province,
                                individu.Code_postal, individu.Programme,
                                individu.Date_debut_programme)
                            );
                        }
                        File.WriteAllLines(path, csvFile);


                        string path2 = fbd.SelectedPath + "/Boutiques Spécialisées.csv";

                        var csvFile2 = new List<string>();
                        foreach (Boutique boutique in boutiques)
                        {
                            csvFile2.Add(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                                boutique.Nom, boutique.Ville,
                                boutique.Rue, boutique.Province,
                                boutique.Code_postal, boutique.Telephone,
                                boutique.Courriel, boutique.Nom_personne_contact,
                                boutique.Pourcentage_rabais
                                )
                            );
                        }
                        File.WriteAllLines(path2, csvFile2);

                        MessageBox.Show("Fichiers CSV enregistrés avec succès !", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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

    public class Individu : INotifyPropertyChanged
    {
        public int Numero { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Ville { get; set; }
        public string Rue { get; set; }
        public string Province { get; set; }
        public string Code_postal { get; set; }
        public int Programme { get; set; }
        public DateTime Date_debut_programme { get; set; }


        public static ObservableCollection<Individu> GetIndividus()
        {
            var individus = new ObservableCollection<Individu>();

            // Load SQL Data
            MySqlConnection maConnexion;
            try
            {
                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT * from individus;";
                MySqlCommand command1 = maConnexion.CreateCommand();
                command1.CommandText = requete;

                MySqlDataReader reader = command1.ExecuteReader();

                string[] valueString = new string[reader.FieldCount];
                while (reader.Read())
                {
                    int _Numero = (int)reader["numero"];
                    string _Nom = (string)reader["nom"];
                    string _Prenom = (string)reader["prenom"];
                    string _Ville = (string)reader["ville"];
                    string _Province = (string)reader["province"];
                    string _Rue = (string)reader["rue"];
                    string _Code_postal = (string)reader["code_postal"];
                    int _Programme = (int)reader["programme"];
                    DateTime _Date_debut_programme = (DateTime)reader["date_debut_programme"];

                    if(_Numero > ClientsView.individus_HighestNumber)
                    {
                        ClientsView.individus_HighestNumber = _Numero;
                    }

                    individus.Add(new Individu()
                    {
                        Numero = _Numero,
                        Nom = _Nom,
                        Prenom = _Prenom,
                        Ville = _Ville,
                        Province = _Province,
                        Rue = _Rue,
                        Code_postal = _Code_postal,
                        Programme = _Programme,
                        Date_debut_programme = _Date_debut_programme,
                    });

                    ClientsView.individus.Add(new Individu()
                    {
                        Numero = _Numero,
                        Nom = _Nom,
                        Prenom = _Prenom,
                        Ville = _Ville,
                        Province = _Province,
                        Rue = _Rue,
                        Code_postal = _Code_postal,
                        Programme = _Programme,
                        Date_debut_programme = _Date_debut_programme,
                    });

                }

                reader.Close();
                command1.Dispose();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }


            return individus;
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

    public class Boutique : INotifyPropertyChanged
    {
        public int Numero { get; set; }
        public string Nom { get; set; }
        public string Ville { get; set; }
        public string Rue { get; set; }
        public string Province { get; set; }
        public string Code_postal { get; set; }
        public string Telephone { get; set; }
        public string Courriel { get; set; }
        public string Nom_personne_contact { get; set; }
        public int Pourcentage_rabais { get; set; }


        public static ObservableCollection<Boutique> GetBoutiques()
        {
            var boutiques = new ObservableCollection<Boutique>();

            // Load SQL Data
            MySqlConnection maConnexion;
            try
            {
                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT * from boutiques_specialisees;";
                MySqlCommand command1 = maConnexion.CreateCommand();
                command1.CommandText = requete;

                MySqlDataReader reader = command1.ExecuteReader();

                string[] valueString = new string[reader.FieldCount];
                while (reader.Read())
                {
                    int _Numero = (int)reader["numero"];
                    string _Nom = (string)reader["nom"];
                    string _Ville = (string)reader["ville"];
                    string _Province = (string)reader["province"];
                    string _Rue = (string)reader["rue"];
                    string _Code_postal = (string)reader["code_postal"];
                    string _Telephone = (string)reader["telephone"];
                    string _Courriel = (string)reader["courriel"];
                    string _Nom_personne_contact = (string)reader["nom_personne_contact"];
                    int _Pourcentage_rabais = (int)reader["pourcentage_rabais"];

                    if (_Numero > ClientsView.boutiques_HighestNumber)
                    {
                        ClientsView.boutiques_HighestNumber = _Numero;
                    }

                    boutiques.Add(new Boutique()
                    {
                        Numero = _Numero,
                        Nom = _Nom,
                        Ville = _Ville,
                        Province = _Province,
                        Rue = _Rue,
                        Code_postal = _Code_postal,
                        Telephone = _Telephone,
                        Courriel = _Courriel,
                        Pourcentage_rabais = _Pourcentage_rabais,
                        Nom_personne_contact = _Nom_personne_contact,
                    });

                    ClientsView.boutiques.Add(new Boutique()
                    {
                        Numero = _Numero,
                        Nom = _Nom,
                        Ville = _Ville,
                        Province = _Province,
                        Rue = _Rue,
                        Code_postal = _Code_postal,
                        Telephone = _Telephone,
                        Courriel = _Courriel,
                        Pourcentage_rabais = _Pourcentage_rabais,
                        Nom_personne_contact = _Nom_personne_contact,
                    });
                }

                reader.Close();
                command1.Dispose();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }


            return boutiques;
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

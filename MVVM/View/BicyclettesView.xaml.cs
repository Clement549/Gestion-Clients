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
    /// <summary>
    /// Logique d'interaction pour OrdersView.xaml
    /// </summary>
    public partial class BicyclettesView : UserControl
    {
        public static List<Bicyclette> bicyclettes = new List<Bicyclette>();

        public static int highestId;

        public static string currentCellValue;
        public static string currentCellHeader;
        public static int currentCellID; // id

        public BicyclettesView()
        {
            InitializeComponent();

            dataGrid.ItemsSource = Bicyclette.GetBicyclettes();

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
                if (e.Column.Header.ToString() == "Id")
                {
                    bicyclettes = bicyclettes.OrderBy(i => i.Id).ToList();
                }
                else if (e.Column.Header.ToString() == "Nom")
                {
                    bicyclettes = bicyclettes.OrderBy(i => i.Nom).ToList();
                }
                else if (e.Column.Header.ToString() == "Grandeur")
                {
                    bicyclettes = bicyclettes.OrderBy(i => i.Grandeur).ToList();
                }
                else if (e.Column.Header.ToString() == "Prix")
                {
                    bicyclettes = bicyclettes.OrderBy(i => i.Prix).ToList();
                }
                else if (e.Column.Header.ToString() == "Ligne produit")
                {
                    bicyclettes = bicyclettes.OrderBy(i => i.Ligne_produit).ToList();
                }
                else if (e.Column.Header.ToString() == "Date introduction")
                {
                    bicyclettes = bicyclettes.OrderBy(i => i.Date_introduction).ToList();
                }
                else if (e.Column.Header.ToString() == "Date discontinuation")
                {
                    bicyclettes = bicyclettes.OrderBy(i => i.Date_discontinuation).ToList();
                }
                else if (e.Column.Header.ToString() == "Assemblage")
                {
                    //bicyclettes = bicyclettes.OrderBy(i => i.Assemblage).ToList();
                }
            }
            else
            {
                if (e.Column.Header.ToString() == "Id")
                {
                    bicyclettes = bicyclettes.OrderByDescending(i => i.Id).ToList();
                }
                else if (e.Column.Header.ToString() == "Nom")
                {
                    bicyclettes = bicyclettes.OrderByDescending(i => i.Nom).ToList();
                }
                else if (e.Column.Header.ToString() == "Grandeur")
                {
                    bicyclettes = bicyclettes.OrderByDescending(i => i.Grandeur).ToList();
                }
                else if (e.Column.Header.ToString() == "Prix")
                {
                    bicyclettes = bicyclettes.OrderByDescending(i => i.Prix).ToList();
                }
                else if (e.Column.Header.ToString() == "Ligne produit")
                {
                    bicyclettes = bicyclettes.OrderByDescending(i => i.Ligne_produit).ToList();
                }
                else if (e.Column.Header.ToString() == "Date introduction")
                {
                    bicyclettes = bicyclettes.OrderByDescending(i => i.Date_introduction).ToList();
                }
                else if (e.Column.Header.ToString() == "Date discontinuation")
                {
                    bicyclettes = bicyclettes.OrderByDescending(i => i.Date_discontinuation).ToList();
                }
                else if (e.Column.Header.ToString() == "Assemblage")
                {
                    //bicyclettes = bicyclettes.OrderByDescending(i => i.Assemblage).ToList();
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

                currentCellID = bicyclettes[e.Row.GetIndex()].Id;

                //MessageBox.Show(highestId.ToString());
            }
            catch (Exception e1) { }
        }

        void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editingTextBox = e.EditingElement as TextBox;
            var newValue = editingTextBox.Text;

            try
            {
                string Query = "UPDATE bicyclettes SET " + e.Column.Header.ToString().Replace(" ", "_") + "='" + newValue + "' WHERE id ='" + currentCellID + "';";
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
                    bicyclettes = bicyclettes.OrderBy(i => i.Id).ToList();
                }

                highestId++;

                string today = DateTime.Now.ToString("yyyy-MM-dd");

                string Query = "INSERT INTO bicyclettes (id, nom, grandeur, prix, ligne_produit, date_introduction, date_discontinuation, assemblage) VALUES ('" + highestId.ToString() + "','','','0','','" + today + "','" + today + "','');";
                MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                MyConn.Open();
                MyCommand2.ExecuteReader();
                MyConn.Close();
                MyCommand2.Dispose();

                await Task.Delay(100);

                bicyclettes.Clear();
                dataGrid.ItemsSource = Bicyclette.GetBicyclettes();
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
                    string Query = "DELETE FROM bicyclettes WHERE id = '" + currentCellID + "';";
                    MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                    MyConn.Open();
                    MyCommand2.ExecuteReader();
                    MyConn.Close();
                    MyCommand2.Dispose();

                    await Task.Delay(100);

                    bicyclettes.Clear();
                    dataGrid.ItemsSource = Bicyclette.GetBicyclettes();
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
                        string path = fbd.SelectedPath + "/Bicyclettes.xml";

                        var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Stock>));

                        var file = new StreamWriter(path);
                        writer.Serialize(file, bicyclettes);
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
                        string path = fbd.SelectedPath + "/Bicyclettes.json";

                        string jsonRepresentation = JsonConvert.SerializeObject(bicyclettes);
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
                        string path = fbd.SelectedPath + "/Bicyclettes.csv";

                        var csvFile = new List<string>();
                        foreach (Bicyclette bicyclette in bicyclettes)
                        {
                            csvFile.Add(string.Format("{0};{1};{2};{3};{4};{5}",
                                bicyclette.Id, bicyclette.Nom,
                                bicyclette.Grandeur, bicyclette.Prix,
                                bicyclette.Date_introduction, bicyclette.Date_discontinuation
                                //bicyclette.Assemblage
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

    public class Bicyclette : INotifyPropertyChanged
    {

        public int Id { get; set; }
        public string Nom { get; set; }
        public string Grandeur { get; set; }
        public float Prix { get; set; }
        public string Ligne_produit { get; set; }
        public DateTime Date_introduction { get; set; }
        public DateTime Date_discontinuation { get; set; }
        //public string Assemblage { get; set; }

        public static ObservableCollection<Bicyclette> GetBicyclettes()
        {
            var bicyclettes = new ObservableCollection<Bicyclette>();

            // Load SQL Data
            MySqlConnection maConnexion;
            try
            {
                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT * from bicyclettes;";
                MySqlCommand command1 = maConnexion.CreateCommand();
                command1.CommandText = requete;

                MySqlDataReader reader = command1.ExecuteReader();

                string[] valueString = new string[reader.FieldCount];
                while (reader.Read())
                {
                    int _Id = (int)reader["id"];
                    string _Nom = (string)reader["nom"];
                    string _Grandeur = (string)reader["grandeur"];
                    float _Prix = (float)reader["prix"];
                    string _Ligne_produit = (string)reader["ligne_produit"];
                    DateTime _Date_introduction = (DateTime)reader["date_introduction"];
                    DateTime _Date_discontinuation = (DateTime)reader["date_discontinuation"];
                    //string _Assemblage = (string)reader["assemblage"];

                    if (_Id > BicyclettesView.highestId)
                    {
                        BicyclettesView.highestId = _Id;
                    }

                    bicyclettes.Add(new Bicyclette()
                    {
                        Id = _Id,
                        Nom = _Nom,
                        Grandeur = _Grandeur,
                        Prix = _Prix,
                        Ligne_produit = _Ligne_produit,
                        Date_introduction = _Date_introduction,
                        Date_discontinuation = _Date_discontinuation,
                        //Assemblage = _Assemblage
                    });

                    BicyclettesView.bicyclettes.Add(new Bicyclette()
                    {
                        Id = _Id,
                        Nom = _Nom,
                        Grandeur = _Grandeur,
                        Prix = _Prix,
                        Ligne_produit = _Ligne_produit,
                        Date_introduction = _Date_introduction,
                        Date_discontinuation = _Date_discontinuation,
                        //Assemblage = _Assemblage
                    });
                }

                reader.Close();
                command1.Dispose();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }


            return bicyclettes;
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

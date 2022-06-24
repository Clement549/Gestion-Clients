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
    /// <summary>
    /// Logique d'interaction pour OrdersView.xaml
    /// </summary>
    public partial class FournisseursView : UserControl
    {
        public static List<Fournisseur> fournisseurs = new List<Fournisseur>();

        public static int HighestNumber;

        public static string currentCellValue;
        public static string currentCellHeader;
        public static int currentCellID; // id

        public FournisseursView()
        {
            InitializeComponent();

            dataGrid.ItemsSource = Fournisseur.GetFournisseurs();

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
                if (e.Column.Header.ToString() == "Siret")
                {
                    fournisseurs = fournisseurs.OrderBy(i => i.Siret).ToList();
                }
                else if (e.Column.Header.ToString() == "Nom")
                {
                    fournisseurs = fournisseurs.OrderBy(i => i.Nom).ToList();
                }
                else if (e.Column.Header.ToString() == "Contact")
                {
                    fournisseurs = fournisseurs.OrderBy(i => i.Contact).ToList();
                }
                else if (e.Column.Header.ToString() == "Rue")
                {
                    fournisseurs = fournisseurs.OrderBy(i => i.Rue).ToList();
                }
                else if (e.Column.Header.ToString() == "Code postal")
                {
                    fournisseurs = fournisseurs.OrderBy(i => i.Code_postal).ToList();
                }
                else if (e.Column.Header.ToString() == "Vile")
                {
                    fournisseurs = fournisseurs.OrderBy(i => i.Ville).ToList();
                }
                else if (e.Column.Header.ToString() == "Province")
                {
                    fournisseurs = fournisseurs.OrderBy(i => i.Province).ToList();
                }
                else if (e.Column.Header.ToString() == "Libelle")
                {
                    fournisseurs = fournisseurs.OrderBy(i => i.Libelle).ToList();
                }
            }
            else
            {
                if (e.Column.Header.ToString() == "Siret")
                {
                    fournisseurs = fournisseurs.OrderByDescending(i => i.Siret).ToList();
                }
                else if (e.Column.Header.ToString() == "Nom")
                {
                    fournisseurs = fournisseurs.OrderByDescending(i => i.Nom).ToList();
                }
                else if (e.Column.Header.ToString() == "Contact")
                {
                    fournisseurs = fournisseurs.OrderByDescending(i => i.Contact).ToList();
                }
                else if (e.Column.Header.ToString() == "Rue")
                {
                    fournisseurs = fournisseurs.OrderByDescending(i => i.Rue).ToList();
                }
                else if (e.Column.Header.ToString() == "Code postal")
                {
                    fournisseurs = fournisseurs.OrderByDescending(i => i.Code_postal).ToList();
                }
                else if (e.Column.Header.ToString() == "Vile")
                {
                    fournisseurs = fournisseurs.OrderByDescending(i => i.Ville).ToList();
                }
                else if (e.Column.Header.ToString() == "Province")
                {
                    fournisseurs = fournisseurs.OrderByDescending(i => i.Province).ToList();
                }
                else if (e.Column.Header.ToString() == "Libelle")
                {
                    fournisseurs = fournisseurs.OrderByDescending(i => i.Libelle).ToList();
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

                currentCellID = fournisseurs[e.Row.GetIndex()].Id;

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
                string Query = "UPDATE fournisseurs SET " + e.Column.Header.ToString().Replace(" ", "_") + "='" + newValue + "' WHERE id ='" + currentCellID + "';";
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
                    fournisseurs = fournisseurs.OrderBy(i => i.Id).ToList();
                }


                HighestNumber++;

                string Query = "INSERT INTO fournisseurs (id, siret, nom, contact, rue, code_postal, ville, province, libelle) VALUES ('" + (HighestNumber).ToString() + "','','','','','','','','0','');";
                MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                MyConn.Open();
                MyCommand2.ExecuteReader();
                MyConn.Close();
                MyCommand2.Dispose();

                await Task.Delay(100);

                fournisseurs.Clear();
                dataGrid.ItemsSource = Fournisseur.GetFournisseurs();
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
                    string Query = "DELETE FROM fournisseurs WHERE id = '" + currentCellID + "';";
                    MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                    MyConn.Open();
                    MyCommand2.ExecuteReader();
                    MyConn.Close();
                    MyCommand2.Dispose();

                    await Task.Delay(100);

                    fournisseurs.Clear();
                    dataGrid.ItemsSource = Fournisseur.GetFournisseurs();
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
                        string path = fbd.SelectedPath + "/Fournisseurs.xml";

                        var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Fournisseur>));

                        var file = new StreamWriter(path);
                        writer.Serialize(file, fournisseurs);
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
                        string path = fbd.SelectedPath + "/Fournisseurs.json";

                        string jsonRepresentation = JsonConvert.SerializeObject(fournisseurs);
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
                        string path = fbd.SelectedPath + "/Fournisseurs.csv";

                        var csvFile = new List<string>();
                        foreach (Fournisseur fournisseur in fournisseurs)
                        {
                            csvFile.Add(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                                fournisseur.Siret, fournisseur.Nom,
                                fournisseur.Rue, fournisseur.Code_postal,
                                fournisseur.Ville, fournisseur.Province,
                                fournisseur.Contact,
                                fournisseur.Libelle
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

        public class Fournisseur : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Siret { get; set; }
        public string Nom { get; set; }
        public string Contact { get; set; }
        public string Rue { get; set; }
        public string Code_postal { get; set; }
        public string Ville { get; set; }
        public string Province { get; set; }
        public int Libelle { get; set; }

        public static ObservableCollection<Fournisseur> GetFournisseurs()
        {
            var fournisseurs = new ObservableCollection<Fournisseur>();

            // Load SQL Data
            MySqlConnection maConnexion;
            try
            {
                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT * from fournisseurs;";
                MySqlCommand command1 = maConnexion.CreateCommand();
                command1.CommandText = requete;

                MySqlDataReader reader = command1.ExecuteReader();

                string[] valueString = new string[reader.FieldCount];
                while (reader.Read())
                {
                    int _Id = (int)reader["id"];
                    string _Siret = (string)reader["siret"];
                    string _Nom = (string)reader["nom"];
                    string _Contact = (string)reader["contact"];
                    string _Rue = (string)reader["rue"];
                    string _Code_postal = (string)reader["code_postal"];
                    string _Ville = (string)reader["ville"];
                    string _Province = (string)reader["province"];
                    int _Libelle = (int)reader["libelle"];

                    if (_Id > FournisseursView.HighestNumber)
                    {
                        FournisseursView.HighestNumber = _Id;
                    }

                    fournisseurs.Add(new Fournisseur()
                    {
                        Id = _Id,
                        Siret = _Siret,
                        Nom = _Nom,
                        Contact = _Contact,
                        Rue= _Rue,
                        Code_postal = _Code_postal,
                        Ville = _Ville,
                        Province = _Province,
                        Libelle = _Libelle,
                    });

                    FournisseursView.fournisseurs.Add(new Fournisseur()
                    {
                        Id = _Id,
                        Siret = _Siret,
                        Nom = _Nom,
                        Contact = _Contact,
                        Rue = _Rue,
                        Code_postal = _Code_postal,
                        Ville = _Ville,
                        Province = _Province,
                        Libelle = _Libelle,
                    });
                }

                reader.Close();
                command1.Dispose();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }


            return fournisseurs;
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

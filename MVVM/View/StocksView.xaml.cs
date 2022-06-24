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
    public partial class StocksView : UserControl
    {
        public static List<Stock> stocks = new List<Stock>();

        public static int highestStockId;

        public static string currentCellValue;
        public static string currentCellHeader;
        public static string currentCellID; // id

        public StocksView()
        {
            InitializeComponent();

            dataGrid.ItemsSource = Stock.GetStocks();

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
                if (e.Column.Header.ToString() == "Numero produit")
                {
                    stocks = stocks.OrderBy(i => i.Numero_produit).ToList();
                }
                else if (e.Column.Header.ToString() == "Description")
                {
                    stocks = stocks.OrderBy(i => i.Description).ToList();
                }
                else if (e.Column.Header.ToString() == "Fournisseur")
                {
                    stocks = stocks.OrderBy(i => i.Fournisseur).ToList();
                }
                else if (e.Column.Header.ToString() == "Prix")
                {
                    stocks = stocks.OrderBy(i => i.Prix).ToList();
                }
                else if (e.Column.Header.ToString() == "Date introduction")
                {
                    stocks = stocks.OrderBy(i => i.Date_introduction).ToList();
                }
                else if (e.Column.Header.ToString() == "Date discontinuation")
                {
                    stocks = stocks.OrderBy(i => i.Date_discontinuation).ToList();
                }
                else if (e.Column.Header.ToString() == "Delais approvisionnement")
                {
                    stocks = stocks.OrderBy(i => i.Delais_approvisionnement).ToList();
                }
                else if (e.Column.Header.ToString() == "Quantite")
                {
                    stocks = stocks.OrderBy(i => i.Quantite).ToList();
                }
            }
            else
            {
                if (e.Column.Header.ToString() == "Numero produit")
                {
                    stocks = stocks.OrderByDescending(i => i.Numero_produit).ToList();
                }
                else if (e.Column.Header.ToString() == "Description")
                {
                    stocks = stocks.OrderByDescending(i => i.Description).ToList();
                }
                else if (e.Column.Header.ToString() == "Fournisseur")
                {
                    stocks = stocks.OrderByDescending(i => i.Fournisseur).ToList();
                }
                else if (e.Column.Header.ToString() == "Prix")
                {
                    stocks = stocks.OrderByDescending(i => i.Prix).ToList();
                }
                else if (e.Column.Header.ToString() == "Date introduction")
                {
                    stocks = stocks.OrderByDescending(i => i.Date_introduction).ToList();
                }
                else if (e.Column.Header.ToString() == "Date discontinuation")
                {
                    stocks = stocks.OrderByDescending(i => i.Date_discontinuation).ToList();
                }
                else if (e.Column.Header.ToString() == "Delais approvisionnement")
                {
                    stocks = stocks.OrderByDescending(i => i.Delais_approvisionnement).ToList();
                }
                else if (e.Column.Header.ToString() == "Quantite")
                {
                    stocks = stocks.OrderByDescending(i => i.Quantite).ToList();
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
                //dataGrid.Columns[0].Visibility = Visibility.Hidden;
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

                currentCellID = stocks[e.Row.GetIndex()].Numero_produit;
            }
            catch (Exception e1) { }
        }

        void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editingTextBox = e.EditingElement as TextBox;
            var newValue = editingTextBox.Text;

            try
            {
                if (currentCellHeader == "Quantite")
                {
                    if (Convert.ToInt32(currentCellValue) > Convert.ToInt32(newValue)) // on a diminué le stock
                    {
                        int qt = Convert.ToInt32(currentCellValue) - Convert.ToInt32(newValue);

                        string message = "Êtes-vous sûre de vouloir jeter " + qt.ToString() + " pièce(s) ?";
                        string caption = "Confirmation";
                        MessageBoxButton buttons = MessageBoxButton.YesNo;
                        MessageBoxImage icon = MessageBoxImage.Question;
                        if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                        {
                            string Query2 = "SELECT prix FROM pieces WHERE numero_produit = '" + currentCellID + "';";
                            MySqlConnection MyConn2 = new MySqlConnection(Variables_Manager.adminLogin);
                            MySqlCommand MyCommand2 = new MySqlCommand(Query2, MyConn2);
                            MyConn2.Open();

                            var reader = MyCommand2.ExecuteReader();
                            float prix = 0;
                            while (reader.Read())
                            {
                                prix = (float)reader["prix"];
                            }

                            MyConn2.Close();
                            MyCommand2.Dispose();

                            //await Task.Delay(100);

                            Variables_Manager.money += (qt * Math.Round(prix)) / 2;
                            App.Current.Dispatcher.Invoke(delegate {
                                MainWindow.moneyTextStatic.Text = "Compte: " + Variables_Manager.money.ToString() + ",00€";
                            });

                            string _Query = "UPDATE manager SET argent = '" + Variables_Manager.money + "';";
                            MySqlConnection _MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                            MySqlCommand _MyCommand = new MySqlCommand(_Query, _MyConn);
                            _MyConn.Open();
                            _MyCommand.ExecuteReader();
                            _MyConn.Close();

                            //await Task.Delay(100);

                            string Query = "UPDATE pieces SET " + e.Column.Header.ToString().Replace(" ", "_") + "='" + newValue + "' WHERE numero_produit ='" + currentCellID + "';";
                            MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                            MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
                            MyConn.Open();
                            MyCommand.ExecuteReader();
                            MyConn.Close();
                        }
                    }
                    else if(Convert.ToInt32(currentCellValue) < Convert.ToInt32(newValue)) // augmente stock
                    {
                        int qt = Convert.ToInt32(newValue) - Convert.ToInt32(currentCellValue);

                        string message = "Êtes-vous sûre de vouloir commander " + qt.ToString()  + " pièce(s) ?";
                        string caption = "Confirmation";
                        MessageBoxButton buttons = MessageBoxButton.YesNo;
                        MessageBoxImage icon = MessageBoxImage.Question;
                        if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                        {
                            string Query2 = "SELECT prix FROM pieces WHERE numero_produit = '" + currentCellID + "';";
                            MySqlConnection MyConn2 = new MySqlConnection(Variables_Manager.adminLogin);
                            MySqlCommand MyCommand2 = new MySqlCommand(Query2, MyConn2);
                            MyConn2.Open();

                            var reader = MyCommand2.ExecuteReader();
                            float prix = 0;
                            while (reader.Read())
                            {
                                prix = (float)reader["prix"];
                            }

                            MyConn2.Close();
                            MyCommand2.Dispose();

                            //await Task.Delay(100);


                            Variables_Manager.money -= qt * Math.Round(prix);

                            // make sure your UI will update even if you are on another thread.
                            App.Current.Dispatcher.Invoke(delegate { 
                                MainWindow.moneyTextStatic.Text = "Compte: " + Variables_Manager.money.ToString() + ",00€";
                            });

                            string _Query = "UPDATE manager SET argent = '" + Variables_Manager.money + "';";
                            MySqlConnection _MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                            MySqlCommand _MyCommand = new MySqlCommand(_Query, _MyConn);
                            _MyConn.Open();
                            _MyCommand.ExecuteReader();
                            _MyConn.Close();

                            //await Task.Delay(100);

                            string Query = "UPDATE pieces SET " + e.Column.Header.ToString().Replace(" ", "_") + "='" + newValue + "' WHERE numero_produit ='" + currentCellID + "';";
                            MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                            MySqlCommand MyCommand = new MySqlCommand(Query, MyConn);
                            MyConn.Open();
                            MyCommand.ExecuteReader();
                            MyConn.Close();
                        }
                    }
                }
                else
                {
                    string Query = "UPDATE pieces SET " + e.Column.Header.ToString().Replace(" ", "_") + "='" + newValue + "' WHERE numero_produit ='" + currentCellID + "';";
                    MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                    MyConn.Open();
                    MyCommand2.ExecuteReader();
                    MyConn.Close();
                }
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
                    stocks = stocks.OrderBy(i => i.Numero_produit).ToList();
                }

                highestStockId++;

                string today = DateTime.Now.ToString("yyyy-MM-dd");

                string Query = "INSERT INTO pieces (numero_produit, description, fournisseur, prix, date_introduction, date_discontinuation, delais_approvisionnement, quantite) VALUES ('Z" + highestStockId.ToString() + "','','','0','" + today + "','" + today + "','0','0');";
                MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                MyConn.Open();
                MyCommand2.ExecuteReader();
                MyConn.Close();
                MyCommand2.Dispose();

                await Task.Delay(100);

                stocks.Clear();
                dataGrid.ItemsSource = Stock.GetStocks();
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
                    string Query = "DELETE FROM pieces WHERE numero_produit = '" + currentCellID + "';";
                    MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                    MyConn.Open();
                    MyCommand2.ExecuteReader();
                    MyConn.Close();
                    MyCommand2.Dispose();

                    await Task.Delay(100);

                    stocks.Clear();
                    dataGrid.ItemsSource = Stock.GetStocks();
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
                        string path = fbd.SelectedPath + "/Stocks.xml";

                        var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Stock>));

                        var file = new StreamWriter(path);
                        writer.Serialize(file, stocks);
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
                        string path = fbd.SelectedPath + "/Stocks.json";

                        string jsonRepresentation = JsonConvert.SerializeObject(stocks);
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
                        string path = fbd.SelectedPath + "/Stocks.csv";

                        var csvFile = new List<string>();
                        foreach (Stock stocks in stocks)
                        {
                            csvFile.Add(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                                stocks.Numero_produit, stocks.Description,
                                stocks.Fournisseur, stocks.Prix, stocks.Date_introduction,
                                stocks.Date_discontinuation, stocks.Delais_approvisionnement,
                                stocks.Quantite
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

    public class Stock : INotifyPropertyChanged
    {

        public string Numero_produit { get; set; }
        public string Description { get; set; }
        public string Fournisseur { get; set; }
        public float Prix { get; set; }
        public DateTime Date_introduction { get; set; }
        public DateTime Date_discontinuation { get; set; }
        public int Delais_approvisionnement { get; set; }
        public int Quantite { get; set; }

        public static ObservableCollection<Stock> GetStocks()
        {
            var stocks = new ObservableCollection<Stock>();

            // Load SQL Data
            MySqlConnection maConnexion;
            try
            {
                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT * from pieces;";
                MySqlCommand command1 = maConnexion.CreateCommand();
                command1.CommandText = requete;

                MySqlDataReader reader = command1.ExecuteReader();

                string[] valueString = new string[reader.FieldCount];
                while (reader.Read())
                {
                    string _Numero_produit = (string)reader["numero_produit"];
                    string _Description = (string)reader["description"];
                    string _Fournisseur = (string)reader["fournisseur"];
                    float _Prix = (float)reader["prix"];
                    DateTime _Date_introduction = (DateTime)reader["date_introduction"];
                    DateTime _Date_discontinuation = (DateTime)reader["date_discontinuation"];
                    int _Delais_approvisionnement = (int)reader["delais_approvisionnement"];
                    int _Quantite = (int)reader["quantite"];

                    stocks.Add(new Stock()
                    {
                        Numero_produit = _Numero_produit,
                        Description = _Description,
                        Fournisseur = _Fournisseur,
                        Prix = _Prix,
                        Date_introduction = _Date_introduction,
                        Date_discontinuation = _Date_discontinuation,
                        Delais_approvisionnement = _Delais_approvisionnement,
                        Quantite = _Quantite
                    });

                    StocksView.stocks.Add(new Stock()
                    {
                        Numero_produit = _Numero_produit,
                        Description = _Description,
                        Fournisseur = _Fournisseur,
                        Prix = _Prix,
                        Date_introduction = _Date_introduction,
                        Date_discontinuation = _Date_discontinuation,
                        Delais_approvisionnement = _Delais_approvisionnement,
                        Quantite = _Quantite
                    });
                }

                reader.Close();
                command1.Dispose();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }


            return stocks;
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

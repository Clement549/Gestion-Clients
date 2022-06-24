using Esilv_BDD;
using Esilv_BDD.MVVM.View;
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
    public partial class OrdersView : UserControl
    {
        public static List<Order> orders = new List<Order>();

        public static int highestOrderId;

        public static string currentCellValue;
        public static string currentCellHeader;
        public static string currentCellID; // id


        public OrdersView()
        {
            InitializeComponent();

            dataGrid.ItemsSource = Order.GetOrders();

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
                    orders = orders.OrderBy(i => i.Id).ToList();
                }
                else if (e.Column.Header.ToString() == "Date commande")
                {
                    orders = orders.OrderBy(i => i.Date_commande).ToList();
                }
                else if (e.Column.Header.ToString() == "Rue")
                {
                    orders = orders.OrderBy(i => i.Rue).ToList();
                }
                else if (e.Column.Header.ToString() == "Code postal")
                {
                    orders = orders.OrderBy(i => i.Code_postal).ToList();
                }
                else if (e.Column.Header.ToString() == "Ville")
                {
                    orders = orders.OrderBy(i => i.Ville).ToList();
                }
                else if (e.Column.Header.ToString() == "Province")
                {
                    orders = orders.OrderBy(i => i.Province).ToList();
                }
                else if (e.Column.Header.ToString() == "Date livraison")
                {
                    orders = orders.OrderBy(i => i.Date_livraison).ToList();
                }
                else if (e.Column.Header.ToString() == "Montant")
                {
                    orders = orders.OrderBy(i => i.Montant).ToList();
                }
            }
            else
            {
                if (e.Column.Header.ToString() == "Id")
                {
                    orders = orders.OrderByDescending(i => i.Id).ToList();
                }
                else if (e.Column.Header.ToString() == "Date commande")
                {
                    orders = orders.OrderByDescending(i => i.Date_commande).ToList();
                }
                else if (e.Column.Header.ToString() == "Rue")
                {
                    orders = orders.OrderByDescending(i => i.Rue).ToList();
                }
                else if (e.Column.Header.ToString() == "Code postal")
                {
                    orders = orders.OrderByDescending(i => i.Code_postal).ToList();
                }
                else if (e.Column.Header.ToString() == "Ville")
                {
                    orders = orders.OrderByDescending(i => i.Ville).ToList();
                }
                else if (e.Column.Header.ToString() == "Province")
                {
                    orders = orders.OrderByDescending(i => i.Province).ToList();
                }
                else if (e.Column.Header.ToString() == "Date livraison")
                {
                    orders = orders.OrderByDescending(i => i.Date_livraison).ToList();
                }
                else if (e.Column.Header.ToString() == "Montant")
                {
                    orders = orders.OrderByDescending(i => i.Montant).ToList();
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

                currentCellID = orders[e.Row.GetIndex()].Id;

                //MessageBox.Show(highestOrderId.ToString());
            }
            catch (Exception e1) { }
        }

        void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var editingTextBox = e.EditingElement as TextBox;
            var newValue = editingTextBox.Text;

            try
            {
                if(currentCellHeader == "Id")
                {
                    string _Query = "UPDATE pieces_commandes SET commande_id = '" + newValue + "' WHERE commande_id ='" + currentCellID + "';";
                    MySqlConnection _MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand _MyCommand = new MySqlCommand(_Query, _MyConn);
                    _MyConn.Open();
                    _MyCommand.ExecuteReader();
                    _MyConn.Close();
                }

                string Query = "UPDATE commandes SET " + e.Column.Header.ToString().Replace(" ", "_") + "='" + newValue + "' WHERE id ='" + currentCellID + "';";
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
                    orders = orders.OrderBy(i => i.Id).ToList();
                }

                highestOrderId++;

                string today = DateTime.Now.ToString("yyyy-MM-dd");

                string Query = "INSERT INTO commandes (id, date_commande, rue, code_postal, ville, province, date_livraison) VALUES ('C" + highestOrderId.ToString()  + "','" + today + "','','','','','" + today +  "');";
                MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                MyConn.Open();
                MyCommand2.ExecuteReader();
                MyConn.Close();
                MyCommand2.Dispose();

                await Task.Delay(100);

                orders.Clear();
                dataGrid.ItemsSource = Order.GetOrders();
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
                    string Query = "DELETE FROM commandes WHERE id = '" + currentCellID + "';";
                    MySqlConnection MyConn = new MySqlConnection(Variables_Manager.adminLogin);
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn);
                    MyConn.Open();
                    MyCommand2.ExecuteReader();
                    MyConn.Close();
                    MyCommand2.Dispose();

                    await Task.Delay(100);

                    orders.Clear();
                    dataGrid.ItemsSource = Order.GetOrders();
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
                        string path = fbd.SelectedPath + "/Commandes.xml";

                        var writer = new System.Xml.Serialization.XmlSerializer(typeof(List<Order>));

                        var file = new StreamWriter(path);
                        writer.Serialize(file, orders);
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
                        string path = fbd.SelectedPath + "/Commandes.json";

                        string jsonRepresentation = JsonConvert.SerializeObject(orders);
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
                        string path = fbd.SelectedPath + "/Commandes.csv";

                        var csvFile = new List<string>();
                        foreach (Order order in orders)
                        {
                            csvFile.Add(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                                order.Id,
                                order.Date_commande, order.Rue,
                                order.Code_postal, order.Ville,
                                order.Province,
                                order.Date_livraison,
                                order.Montant
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

    public class Order : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public DateTime Date_commande { get; set; }
        public string Rue { get; set; }
        public string Code_postal { get; set; }
        public string Ville { get; set; }
        public string Province { get; set; }
        public DateTime Date_livraison { get; set; }

        public double Montant { get; set; }


        public static ObservableCollection<Order> GetOrders()
        {
            var orders = new ObservableCollection<Order>();

            // Load SQL Data
            MySqlConnection maConnexion;
            try
            {
                maConnexion = new MySqlConnection(Variables_Manager.adminLogin);
                maConnexion.Open();

                string requete = "SELECT * from commandes;";
                MySqlCommand command1 = maConnexion.CreateCommand();
                command1.CommandText = requete;

                MySqlDataReader reader = command1.ExecuteReader();

                string[] valueString = new string[reader.FieldCount];
                while (reader.Read())
                {

                    string _Id = (string)reader["id"];
                    DateTime _Date_commande = (DateTime)reader["date_commande"];
                    string _Rue = (string)reader["rue"];
                    string _Code_postal = (string)reader["code_postal"];
                    string _Ville = (string)reader["ville"];
                    string _Province = (string)reader["province"];
                    DateTime _Date_livraison = (DateTime)reader["date_livraison"];
                    double _Montant = (double)reader["montant"];

                    if (_Id.Length == 2)
                    {
                        if (Convert.ToInt32(_Id.Substring(_Id.Length - 1)) > OrdersView.highestOrderId)
                        {

                            OrdersView.highestOrderId = Convert.ToInt32(_Id.Substring(_Id.Length - 1));
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(_Id.Substring(_Id.Length - 2)) > OrdersView.highestOrderId)
                        {

                            OrdersView.highestOrderId = Convert.ToInt32(_Id.Substring(_Id.Length - 2));
                        }
                    }

                    orders.Add(new Order()
                    {
                        Id = _Id,
                        Date_commande = _Date_commande,
                        Rue = _Rue,
                        Code_postal = _Code_postal,
                        Ville = _Ville,
                        Province = _Province,
                        Date_livraison = _Date_livraison,
                        Montant = _Montant,
                    });

                    OrdersView.orders.Add(new Order()
                    {
                        Id = _Id,
                        Date_commande = _Date_commande,
                        Rue = _Rue,
                        Code_postal = _Code_postal,
                        Ville = _Ville,
                        Province = _Province,
                        Date_livraison = _Date_livraison,
                        Montant = _Montant,
                    });
                }

                reader.Close();
                command1.Dispose();
            }
            catch (MySqlException e)
            {
                Console.Write("Erreur de Connexion : " + e.ToString());
            }


            return orders;
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

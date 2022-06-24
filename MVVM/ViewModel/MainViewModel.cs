using Esilv_BDD.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esilv_BDD.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand StatsViewCommand { get; set; }
        public RelayCommand OrdersViewCommand { get; set; }
        public RelayCommand AssemblagesViewCommand { get; set; }
        public RelayCommand ClientsViewCommand { get; set; }
        public RelayCommand FournisseursViewCommand { get; set; }
        public RelayCommand StocksViewCommand { get; set; }
        public RelayCommand ProgrammesViewCommand { get; set; }
        public RelayCommand WebViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand BicyclettesViewCommand { get; set; }
        public RelayCommand CreateOrderViewCommand { get; set; }

        public StatsViewModel StatsVM { get; set; }
        public OrdersViewModel OrdersVM { get; set; }
        public AssemblagesViewModel AssemblagesVM { get; set; }
        public ClientsViewModel ClientsVM { get; set; }
        public FournisseursViewModel FournisseursVM { get; set; }
        public StocksViewModel StocksVM { get; set; }
        public ProgrammesViewModel ProgrammesVM { get; set; }
        public WebViewModel WebVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public BicyclettesViewModel BicyclettesVM { get; set; }
        public CreateOrderViewModel CreateOrderVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                onPropertyChanged();
            }
        }
        public MainViewModel()
        {
            StatsVM = new StatsViewModel();
            OrdersVM = new OrdersViewModel();
            AssemblagesVM = new AssemblagesViewModel();
            ClientsVM = new ClientsViewModel();
            FournisseursVM = new FournisseursViewModel();
            StocksVM = new StocksViewModel();
            ProgrammesVM = new ProgrammesViewModel();
            WebVM = new WebViewModel();
            SettingsVM = new SettingsViewModel();
            BicyclettesVM = new BicyclettesViewModel();
            CreateOrderVM = new CreateOrderViewModel();

            CurrentView = StatsVM;

            StatsViewCommand = new RelayCommand(o =>
            {
                CurrentView = StatsVM;
            });
            OrdersViewCommand = new RelayCommand(o =>
            {
                CurrentView = OrdersVM;
            });
            AssemblagesViewCommand = new RelayCommand(o =>
            {
                CurrentView = AssemblagesVM;
            });
            ClientsViewCommand = new RelayCommand(o =>
            {
                CurrentView = ClientsVM;
            });
            FournisseursViewCommand = new RelayCommand(o =>
            {
                CurrentView = FournisseursVM;
            });
            StocksViewCommand = new RelayCommand(o =>
            {
                CurrentView = StocksVM;
            });
            ProgrammesViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProgrammesVM;
            });
            WebViewCommand = new RelayCommand(o =>
            {
                CurrentView = WebVM;
            });
            SettingsViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingsVM;
            });
            BicyclettesViewCommand = new RelayCommand(o =>
            {
                CurrentView = BicyclettesVM;
            });
            CreateOrderViewCommand = new RelayCommand(o =>
            {
                CurrentView = CreateOrderVM;
            });
        }
    }
}

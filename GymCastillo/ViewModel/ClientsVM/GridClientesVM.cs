using GymCastillo.Model.DataTypes;
using log4net;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GymCastillo.ViewModel.Commands.ClientsCommands;
using System.Windows;
using System.Windows.Documents;
using GymCastillo.Model.Database;
using GymCastillo.Model.Helpers;
using GymCastillo.View.ClientsView;

namespace GymCastillo.ViewModel.ClientsVM {
    public class GridClientesVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Cliente> clientes { get; set; }
        
        public ObservableCollection<Cliente> ClientesLista { get; set; }

        public NewClientWindowCommand newClient { get; set; }

        public OverViewClienteCommand overViewCommand { get; set; }

        private Cliente selectedClient;
        public Cliente SelectedClient {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                if (selectedClient != null) {
                    OnPropertyChanged(nameof(SelectedClient));
                }
            }
        }

        private string query;

        public string Query {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged(nameof(Query));
                FilterList(query);
            }
        }

        public GridClientesVM() {
            try {
                clientes = GetFromDb.GetClientes().Result;
                ClientesLista = new ObservableCollection<Cliente>();
                overViewCommand = new(this);
                newClient = new(this);

                foreach (var cliente in clientes) {
                    ClientesLista.Add(cliente);
                }

                Log.Debug("Se ha inicializado y se han obtenido los datos de la pantalla de GridClientes.");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al iniciar la pantalla de GridClientes.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al iniciar la pantalla de GridClientes. Error: {e.Message}",
                    "Error Desconocido");
            }
        }

        public void OpenOverview() {
            OverviewClientsWindow window = new OverviewClientsWindow(selectedClient);
            window.ShowDialog();
            Log.Debug("Ventana de overview iniciada");
        }

        public void OpenNewClients() {
            NewClientsWindow window = new();
            window.ShowDialog();
            Log.Debug("Ventanta de nuevo usuario iniciada");
        }

        private void FilterList(string query) {

            if (clientes != null) {
                if (query == "") {
                    ClientesLista.Clear();
                    foreach (var cliente in clientes) {
                        ClientesLista.Add(cliente);
                    }
                }
                else {
                    ClientesLista.Clear();
                    var filteredList = clientes.Where(c => c.Nombre.ToLower().Contains(query.ToLower())).ToList();
                    foreach (var cliente in filteredList) {
                        ClientesLista.Add(cliente);
                    }
                }
            }
            else return;
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

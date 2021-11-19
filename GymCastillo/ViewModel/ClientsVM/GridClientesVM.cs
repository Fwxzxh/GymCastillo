using GymCastillo.Model.DataTypes;
using log4net;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GymCastillo.ViewModel.Commands;
using System.Windows;
using GymCastillo.Model.Database;
using GymCastillo.Model.Helpers;
using GymCastillo.View.ClientsView;

namespace GymCastillo.ViewModel.ClientsVM {
    public class GridClientesVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;


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

        public OverViewCommand overViewCommand { get; set; }

        public NewClientWindowCommand clientCommand { get; set; }

        private List<Cliente> clientes = GetFromDb.GetClientes().Result;

        public ObservableCollection<Cliente> ClientesLista { get; set; }

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
                ClientesLista = new ObservableCollection<Cliente>();
                clientCommand = new();
                overViewCommand = new(this);

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
            window.Show();
        }

        private void FilterList(string query) {
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

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

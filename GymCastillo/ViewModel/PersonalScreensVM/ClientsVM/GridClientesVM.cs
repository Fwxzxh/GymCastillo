using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.View.PersonalScreenView.ClientsView;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientsCommands;
using log4net;

namespace GymCastillo.ViewModel.PersonalScreensVM.ClientsVM {
    public class GridClientesVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Cliente> clientes { get; set; }
        
        public ObservableCollection<Cliente> ClientesLista { get; set; }

        public NewClientWindowCommand newClient { get; set; }

        public DeleteClientCommand deleteClient { get; set; }

        public OverViewClienteCommand OverViewCommand { get; set; }

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
                //FilterList(query);
            }
        }

        public GridClientesVM() {
            try {
                //clientes = InitInfo.ObCoClientes;
                //ClientesLista = new ObservableCollection<Cliente>();
                OverViewCommand = new(this);
                newClient = new(this);
                deleteClient = new(this);

                Log.Debug("Se ha inicializado y se han obtenido los datos de la pantalla de GridClientes.");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al iniciar la pantalla de GridClientes.");
                Log.Error($"Error: {e.Message}");
                //ShowPrettyMessages.ErrorOk(
                //    $"Ha ocurrido un error desconocido al iniciar la pantalla de GridClientes. Error: {e.Message}",
                //    "Error Desconocido");
            }
        }

        public void OpenOverview() {
            OverviewClientsWindow window = new OverviewClientsWindow(selectedClient);
            window.ShowDialog();
            RefreshGrid();
            Log.Debug("Ventana de overview iniciada");
        }

        public void OpenNewClients() {
            Log.Debug("Ventana de nuevo usuario iniciada");
            NewClientsWindow window = new();
            window.ShowDialog();
            RefreshGrid();
        }

        public async void DeleteClient() {
            if (ShowPrettyMessages.QuestionYesNo("¿Desea borrar el cliente?", "Confirmación")) {
                await AdminUsuariosGeneral.Delete(SelectedClient);
                RefreshGrid();
            }
            else return;
        }

        private async void RefreshGrid() {
            InitInfo.ObCoClientes.Clear();
            var clientesRe = await GetFromDb.GetClientes();
            foreach (var item in clientesRe.Where(c=> c.Activo == true)) {
                InitInfo.ObCoClientes.Add(item);
            }
        }

        //private async void FilterList(string query) {
        //    clientes = await GetFromDb.GetClientes();
        //    if (clientes != null) {  
        //        if (string.IsNullOrWhiteSpace(query)) {
        //            ClientesLista.Clear();
        //            foreach (var cliente in clientes.OrderBy(c => c.Nombre)) {
        //                ClientesLista.Add(cliente);
        //            }
        //        }
        //        else {
        //            ClientesLista.Clear();
        //            var filteredList = clientes.Where(c => c.Nombre.ToLower().Contains(query.ToLower()) || c.ApellidoPaterno.ToLower().Contains(query.ToLower()) || c.ApellidoMaterno.ToLower().Contains(query.ToLower())).ToList().OrderBy(d => d.Nombre);
        //            foreach (var cliente in filteredList) {
        //                ClientesLista.Add(cliente);
        //            }
        //        }
        //    }
        //    else return;
        //}

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

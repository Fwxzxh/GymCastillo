using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
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
        public RelayCommand EliminarInactivosCommand { get; set; }

        private bool activo = false;

        public bool Activo {
            get { return activo; }
            set
            {
                activo = value;
                OnPropertyChanged(nameof(Activo));
                RefreshGrid(value);
            }
        }

        private int totalActivos;

        public int TotalActivos {
            get { return totalActivos; }
            set
            {
                totalActivos = value;
                OnPropertyChanged(nameof(TotalActivos));
            }
        }


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

        private string query = "";

        public string Query {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged(nameof(Query));
                FilterData(value);
            }
        }

        private static void FilterData(string value) {
            if (value != null) {
                CollectionViewSource.GetDefaultView(InitInfo.ObCoClientes).Filter = item => (item as Cliente).Nombre.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
            }
            else CollectionViewSource.GetDefaultView(InitInfo.ObCoClientes);
        }

        public GridClientesVM() {
            try {
                RefreshGrid(Activo);
                OverViewCommand = new(this);
                newClient = new(this);
                deleteClient = new(this);
                EliminarInactivosCommand = new RelayCommand(EliminarInactivos);
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

        private async void  EliminarInactivos() {
            await DeleteWithTime();
            RefreshGrid(Activo);
        }

        public static async Task DeleteWithTime() {
            var fechaLimite = DateTime.Today - TimeSpan.FromDays(120); // 120 dias = ~4 meses, pero puede ser configurable

            // Obtenemos los clientes cuya fecha de ultimo acceso sea hace más de 4 meses
            var clientes =
                InitInfo.ObCoClientes.Where(x => x.FechaUltimoAcceso > fechaLimite);

            try {
                foreach (var cliente in clientes) {
                    await cliente.Delete();
                }
                ShowPrettyMessages.InfoOk("Clientes inactivos eliminados correctamente.", "Eliminar Clientes Inactivos");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error desconocido a la hora de hacer el borrado de clientes inactivos.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Ha ocurrido un error desconocido al eliminar los clientes inactivos, Error: {e.Message}",
                    "Error desconocido");
            }
        }
        public void OpenOverview() {
            OverviewClientsWindow window = new OverviewClientsWindow(selectedClient);
            window.ShowDialog();
            RefreshGrid(activo);
            Log.Debug("Ventana de overview iniciada");
        }

        public void OpenNewClients() {
            Log.Debug("Ventana de nuevo usuario iniciada");
            NewClientsWindow window = new();
            window.ShowDialog();
            RefreshGrid(activo);
        }

        public async void DeleteClient() {
            if (ShowPrettyMessages.QuestionYesNo("¿Desea borrar el cliente?", "Confirmación")) {
                await AdminUsuariosGeneral.Delete(SelectedClient);
                RefreshGrid(activo);
            }
            else return;
        }

        private async void RefreshGrid(bool value) {
            TotalActivos = InitInfo.ObCoClientes.Where(c => c.Activo).Count();
            var lista = await GetFromDb.GetClientes();
            InitInfo.ObCoClientes.Clear();
            if (value) {
                foreach (var item in lista.Where(c => c.Activo == true)) {
                    InitInfo.ObCoClientes.Add(item);
                }
            }
            else {
                foreach (var item in lista) {
                    InitInfo.ObCoClientes.Add(item);
                }
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

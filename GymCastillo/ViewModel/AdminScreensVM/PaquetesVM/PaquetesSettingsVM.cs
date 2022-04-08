using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.View.AdminScreensView.PaquetesView;
using log4net;

namespace GymCastillo.ViewModel.AdminScreensVM.PaquetesVM {
    public class PaquetesSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand OpenClases { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public ObservableCollection<Paquete> ListaPaquetes { get; set; }

        private ObservableCollection<Clase> clases;

        public ObservableCollection<Clase> ListaClases {
            get { return clases; }
            set
            {
                clases = value;
                OnPropertyChanged(nameof(ListaClases));
            }
        }

        private Paquete paquete = new();

        public Paquete Paquete {
            get { return paquete; }
            set
            {
                paquete = value;
                OnPropertyChanged(nameof(Paquete));
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
                CollectionViewSource.GetDefaultView(InitInfo.ObCoDePaquetes).Filter = item => (item as Paquete).NombrePaquete.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
            }
            else CollectionViewSource.GetDefaultView(InitInfo.ObCoDePaquetes);
        }
        public PaquetesSettingsVM() {
            try {
                Log.Debug("Ventana de configuración de paquetes iniciada");
                ListaPaquetes = new ObservableCollection<Paquete>(InitInfo.ObCoDePaquetes);
                ListaClases = new ObservableCollection<Clase>(InitInfo.ObCoClases);
                SaveCommand = new RelayCommand(SavePaquete);
                CancelCommand = new RelayCommand(CancelarPaquete);
                DeleteCommand = new RelayCommand(DeletePaquete);
                OpenClases = new RelayCommand(ConfigurarClases);
            }
            catch (Exception e) {
                Log.Error("Error en el vm de paquetes");
                Log.Error(e.Message);
                //ShowPrettyMessages.ErrorOk($"Error en la ventana de paquetes {e.Message}", "Error");
            }
        }

        private void ConfigurarClases() {
            PaquetesClasesWindow window = new(Paquete);
            window.ShowDialog();
            RefreshGrid();
        }

        private async void DeletePaquete() {
            if (ShowPrettyMessages.QuestionYesNo("¿Desea eliminar el paquete?", "Confirmación")) {
                await AdminOtrosTipos.Delete(Paquete);
            }
            RefreshGrid();
        }

        private void CancelarPaquete() {
            RefreshGrid();
        }

        private async void SavePaquete() {
            Log.Debug("Se ha presionado el botón para guardar un paquete");
            await AdminOtrosTipos.Alta(Paquete);
            RefreshGrid();

        }

        private async void RefreshGrid() {
            ListaPaquetes.Clear();
            ListaClases.Clear();
            InitInfo.ObCoDePaquetes.Clear();
            var paquetes = await GetFromDb.GetPaquetes();
            foreach (var item in paquetes) {
                InitInfo.ObCoDePaquetes.Add(item);
            }
            Paquete = null;
            Paquete = new();
        }
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

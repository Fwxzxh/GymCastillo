using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.SettingsScreensVM.PaquetesVM {
    public class PaquetesSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
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

        public PaquetesSettingsVM() {
            try {
                Log.Debug("Ventana de configuración de paquetes iniciada");
                ListaPaquetes = new ObservableCollection<Paquete>(InitInfo.ListaDePaquetes);
                ListaClases = new ObservableCollection<Clase>(InitInfo.ListaClases);
                SaveCommand = new RelayCommand(SavePaquete);
                CancelCommand = new RelayCommand(CancelarPaquete);
                DeleteCommand = new RelayCommand(DeletePaquete);
            }
            catch (Exception e) {
                Log.Error("Error en el vm de paquetes");
                Log.Error(e.Message);
                //ShowPrettyMessages.ErrorOk($"Error en la ventana de paquetes {e.Message}", "Error");
            }
        }

        private async void DeletePaquete() {
            if (ShowPrettyMessages.QuestionYesNo("¿Desea eliminar el paquete?", "Confirmación")) {
                await AdminOtrosTipos.Delete(Paquete);
            }
            RefreshGrid();
        }

        private void CancelarPaquete() {
            Paquete = null;
            Paquete = new();
            RefreshGrid();
        }

        private async void SavePaquete() {
            Log.Debug("Se ha presionado el botón para guardar un paquete");
            await AdminOtrosTipos.Alta(Paquete);
            RefreshGrid();
        }

        public async void RefreshGrid() {
            ListaPaquetes.Clear();
            ListaClases.Clear();
            //ListaClases = new ObservableCollection<Clase>(InitInfo.ListaClases.Where(c => c.Activo == true));
            var paquetes = await GetFromDb.GetPaquetes();
            var clases = await GetFromDb.GetClases();
            foreach (var item in paquetes) {
                ListaPaquetes.Add(item);
            }

            //foreach (var item in clases.Where(c => c.Activo == true)) {
            //    ListaClases.Add(item);
            //}
        }
    }
}

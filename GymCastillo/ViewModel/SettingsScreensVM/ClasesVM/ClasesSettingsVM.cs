using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace GymCastillo.ViewModel.SettingsScreensVM.ClasesVM {
    public class ClasesSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Clase> ListaClases { get; set; }
        public ObservableCollection<Instructor> ListaInstructores { get; set; }
        public ObservableCollection<Espacio> ListaEspacios { get; set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand<bool> SaveCommand { get; private set; }

        private Clase clase = new();
        private bool claseActiva;

        public Clase Clase {
            get { return clase; }
            set
            {
                clase = value;
                OnPropertyChanged(nameof(Clase));
            }
        }

        public bool ClaseActiva {
            get { return claseActiva; }
            set
            {
                claseActiva = value;
                OnPropertyChanged(nameof(ClaseActiva));
                RefreshGrid(ClaseActiva);
            }
        }


        public ClasesSettingsVM() {
            try {
                Log.Debug("Iniciando settings de clases.");
                ListaClases = new ObservableCollection<Clase>(InitInfo.ListaClases);
                ListaInstructores = new ObservableCollection<Instructor>(InitInfo.ListaInstructor);
                ListaEspacios = new ObservableCollection<Espacio>(InitInfo.ListEspacios);
                CancelCommand = new RelayCommand(CancelUpdate);
                DeleteCommand = new RelayCommand(DeleteClass);
                SaveCommand = new RelayCommand<bool>(SaveClass);
            }
            catch (Exception e) {
                Log.Error(e.Message);
                //ShowPrettyMessages.ErrorOk(e.Message, "Error");
            }
        }

        private async void SaveClass(bool guardar) {
            if (string.IsNullOrEmpty(Clase.NombreClase) ||
                string.IsNullOrEmpty(Clase.Descripcion) ||
                Clase.IdEspacio == 0 ||
                Clase.IdInstructor == 0) {
                return;
            }
            else {
                if (guardar) {
                    Log.Debug($"Alta de la clase {Clase.NombreClase} ");
                    await AdminOtrosTipos.Alta(Clase);
                }
                else {
                    Log.Debug($"Update de la clase {Clase.NombreClase}");
                    await AdminOtrosTipos.Update(Clase);
                }
            }

            RefreshGrid(claseActiva);

        }

        private void CancelUpdate() {
            RefreshGrid(claseActiva);
        }

        private async void DeleteClass() {
            if (ShowPrettyMessages.QuestionYesNo("¿Desea eliminar la clase?", "Confirmación")) {
                await AdminOtrosTipos.Delete(Clase);
                RefreshGrid(claseActiva);
            }
            else {
                RefreshGrid(claseActiva);
                return;
            }
        }

        private async void RefreshGrid(bool activa) {
            Clase = null;
            Clase = new();
            ListaClases.Clear();
            var clases = await GetFromDb.GetClases();
            if (activa) {
                foreach (var item in clases.Where(c => c.Activo == true)) {
                    ListaClases.Add(item);
                }
            }
            else {
                foreach (var item in clases) {
                    ListaClases.Add(item);
                }
            }

        }
    }
}

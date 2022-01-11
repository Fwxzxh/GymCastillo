using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.View.AdminScreensView.ClasesView;
using log4net;

namespace GymCastillo.ViewModel.AdminScreensVM.ClasesVM {
    public class ClasesSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Clase> ListaClases { get; set; }
        public ObservableCollection<Instructor> ListaInstructores { get; set; }

        //public InitTest lista { get; set; }
        public ObservableCollection<Espacio> ListaEspacios { get; set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }

        public RelayCommand OpenHorarios { get;  private set; }
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

        private string query = "";

        public string Query {
            get { return query; }
            set { query = value;
                OnPropertyChanged(nameof(Query));
                FilterData(value);
            }
        }
        private static void FilterData(string value) {
            if (value != null) {
                CollectionViewSource.GetDefaultView(InitInfo.ObCoClases).Filter = item => (item as Clase).NombreClase.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
            }
            else CollectionViewSource.GetDefaultView(InitInfo.ObCoClases);
        }

        public ClasesSettingsVM() {
            try {
                Log.Debug("Iniciando settings de clases.");
                ListaEspacios = new ObservableCollection<Espacio>(InitInfo.ObCoEspacios);
                CancelCommand = new RelayCommand(CancelUpdate);
                DeleteCommand = new RelayCommand(DeleteClass);
                SaveCommand = new RelayCommand<bool>(SaveClass);
                OpenHorarios = new RelayCommand(OpenHorariosW);
            }
            catch (Exception e) {
                Log.Error(e.Message);
                //ShowPrettyMessages.ErrorOk(e.Message, "Error");
            }
        }

        private void OpenHorariosW() {
            HorariosWindow horarios = new(Clase);
            horarios.ShowDialog();
            Clase = null;
            Clase = new();
        }

        private async void SaveClass(bool guardar) {
            if (string.IsNullOrEmpty(Clase.NombreClase) ||
                string.IsNullOrEmpty(Clase.Descripcion) ||
                Clase.IdEspacio == 0
                // TODO: Bry, Esto ya no va ya que instructores funciona diferente.
                //|| Clase.IdInstructor == 0
                ) {
                return;
            }
            else {
                if (guardar) {
                    Log.Debug($"Alta de la clase {Clase.NombreClase} ");
                    await AdminOtrosTipos.Alta(Clase);
                    var clases = await GetFromDb.GetClases();
                    InitInfo.ObCoClases.Clear();
                    foreach (var item in clases) {
                        InitInfo.ObCoClases.Add(item);
                    }

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

        public async void RefreshGrid(bool activa) {
            Query = "";
            Clase = null;
            Clase = new();
            var clases = await GetFromDb.GetClases();
            InitInfo.ObCoClases.Clear();
            if (activa) {
                foreach (var item in clases.Where(c => c.Activo == true)) {
                    InitInfo.ObCoClases.Add(item);
                }
            }
            else {
                foreach (var item in clases) {
                    InitInfo.ObCoClases.Add(item);
                }
            }

        }
    }
}

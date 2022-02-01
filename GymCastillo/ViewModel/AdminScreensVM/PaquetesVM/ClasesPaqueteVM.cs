using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Init;
using log4net;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GymCastillo.Model.DataTypes.IntersectionTables;

namespace GymCastillo.ViewModel.AdminScreensVM.PaquetesVM {
    public class ClasesPaqueteVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand AddCommand { get; set; }

        private Clase selectedClase = new();

        public Clase SelectedClase {
            get { return selectedClase; }
            set
            {
                selectedClase = value;
                OnPropertyChanged(nameof(SelectedClase));
            }
        }

        private ObservableCollection<PaquetesClases> clases;

        public ObservableCollection<PaquetesClases> Clases {
            get { return clases; }
            set
            {
                clases = value;
                OnPropertyChanged(nameof(Clases));
            }
        }

        private ObservableCollection<Clase> listaClases;

        public ObservableCollection<Clase> ListaClases {
            get { return listaClases; }
            set
            {
                listaClases = value;
                OnPropertyChanged(nameof(ListaClases));
            }
        }

        private Paquete paquete;

        public Paquete Paquete {
            get { return paquete; }
            set
            {
                paquete = value;
                OnPropertyChanged(nameof(Paquete));
            }
        }

        private PaquetesClases paquetesClases = new();

        public PaquetesClases PaquetesClases {
            get { return paquetesClases; }
            set
            {
                paquetesClases = value;
                OnPropertyChanged(nameof(PaquetesClases));
            }
        }

        public ClasesPaqueteVM(Paquete paquete) {
            Clases = new ObservableCollection<PaquetesClases>();
            ListaClases = new ObservableCollection<Clase>();
            this.paquete = paquete;
            DeleteCommand = new RelayCommand(DeleteClase);
            AddCommand = new RelayCommand(AddClase);
            RefreshGrid();
            Log.Debug($"Ventana de configuración para el paquete {paquete.NombrePaquete} iniciada");
        }

        private async void AddClase() {
            //paquetesClases.IdClase = 0;
            paquetesClases.IdClase = SelectedClase.IdClase;
            paquetesClases.IdPaquete = Paquete.IdPaquete;
            await AdminOtrosTipos.Alta(paquetesClases, true);
            RefreshGrid();
            Log.Debug($"Nueva clase agregada al paquete {paquete.NombrePaquete}");
        }

        private async void RefreshGrid() {
            InitInfo.ListPaquetesClases.Clear();
            InitInfo.ObCoClases.Clear();
            ListaClases.Clear();
            var listaActiva = await GetFromDb.GetClases();
            var listaclases = await GetFromDb.GetPaquetesClases();
            foreach (var item in listaclases) {
                InitInfo.ListPaquetesClases.Add(item);
            }
            foreach (var item in listaActiva.Where(c => c.Activo == true)) {
                InitInfo.ObCoClases.Add(item);
                ListaClases.Add(item);
            }

            if (clases != null) {
                clases.Clear();
            }

            foreach (var clase in listaclases.Where(c => c.IdPaquete == paquete.IdPaquete)) {
                clases.Add(clase);
            }
            PaquetesClases = new();
        }

        private async void DeleteClase() {
            await AdminOtrosTipos.Delete(paquetesClases, true);
            RefreshGrid();
            Log.Debug($"Clase borrada al paquete {paquete.NombrePaquete}");
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

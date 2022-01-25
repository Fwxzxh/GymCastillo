using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.IntersectionTables;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.AdminScreensVM.ClasesVM {
    public class InstructoresSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Clase clase;

        public Clase Clase {
            get { return clase; }
            set
            {
                clase = value;
                OnPropertyChanged(nameof(Clase));
            }
        }

        private Instructor instructor;

        public Instructor Instructor {
            get { return instructor; }
            set
            {
                instructor = value;
                OnPropertyChanged(nameof(Instructor));
            }
        }

        private ObservableCollection<ClaseInstructores> listaInstructor;

        public ObservableCollection<ClaseInstructores> ListaInstructor {
            get { return listaInstructor; }
            set
            {
                listaInstructor = value;
                OnPropertyChanged(nameof(ListaInstructor));
            }
        }

        public InstructoresSettingsVM(Clase clase) {
            this.clase = clase;
            ListaInstructor = new ObservableCollection<ClaseInstructores>();
            AddCommand = new RelayCommand(AgregarInstructor);
            DeleteCommand = new RelayCommand(BorrarInstructor);
            RefreshGrid();
        }

        private async void BorrarInstructor() {
            ClaseInstructores instructores = new();
            instructores.IdClase = clase.IdClase;
            instructores.IdInstructor = instructor.Id;
            await AdminOtrosTipos.Delete(instructores);
            RefreshGrid();
        }

        private async void RefreshGrid() {  
            InitInfo.ObCoClaseInstructores.Clear();
            if (ListaInstructor != null) ListaInstructor.Clear();

            var lista = await GetFromDb.GetClaseInstructores();
            foreach (var item in lista.Where(l => l.IdClase == clase.IdClase)) {
                ListaInstructor.Add(item);
            }

            foreach (var item in lista) {
                InitInfo.ObCoClaseInstructores.Add(item);
            }

        }

        private async void AgregarInstructor() {
            ClaseInstructores instructores = new();
            instructores.IdClase = clase.IdClase;
            instructores.IdInstructor = instructor.Id;
            await AdminOtrosTipos.Alta(instructores);
            RefreshGrid();
        }
    }
}

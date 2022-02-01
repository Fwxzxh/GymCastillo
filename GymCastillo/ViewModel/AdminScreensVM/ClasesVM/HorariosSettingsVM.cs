using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using log4net;

namespace GymCastillo.ViewModel.AdminScreensVM.ClasesVM {
    public class HorariosSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
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

        private int dia;

        public int Dia {
            get { return dia; }
            set
            {
                dia = value;
                OnPropertyChanged(nameof(Dia));
            }
        }

        private Horario horario = new();

        public Horario Horarios {
            get { return horario; }
            set
            {
                horario = value;
                OnPropertyChanged(nameof(Horarios));
            }
        }

        private Horario selectedHorario = new();

        public Horario SelectedHorario {
            get { return selectedHorario; }
            set
            {
                selectedHorario = value;
                OnPropertyChanged(nameof(SelectedHorario));
            }
        }


        public RelayCommand<IClosable> CloseCommand { get; private set; }

        public RelayCommand AgregarCommand { get; set; }

        public RelayCommand DeleteCommand { get; set; }

        public ObservableCollection<Horario> ListaHorarios { get; set; }

        public HorariosSettingsVM(Clase clase) {
            this.clase = clase;
            ListaHorarios = new ObservableCollection<Horario>();
            HorariosClase(clase.IdClase);
            CloseCommand = new RelayCommand<IClosable>(this.CloseWindow);
            AgregarCommand = new RelayCommand(AgregarHorario);
            DeleteCommand = new RelayCommand(BorrarHorario);
        }

        private async void BorrarHorario() {
            await AdminOtrosTipos.Delete(SelectedHorario, true);
            HorariosClase(clase.IdClase);
        }

        private async void AgregarHorario() {
            Horarios.Dia = dia + 1;
            Horarios.IdClase = clase.IdClase;
            await AdminOtrosTipos.Alta(Horarios, true);
            HorariosClase(clase.IdClase);
        }

        private async void HorariosClase(int id) {
            if (ListaHorarios != null) {
                ListaHorarios.Clear();
            }
            var horarios = await GetFromDb.GetHorarios();
            InitInfo.ObCoHorarios.Clear();
            foreach (var item in horarios) {
                InitInfo.ObCoHorarios.Add(item);
            }
            foreach (var item in horarios.Where(h => h.IdClase == id).OrderBy(dia => dia.Dia)) {
                ListaHorarios.Add(item);
            }
        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }
    }
}

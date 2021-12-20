using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.Helpers;
using GymCastillo.View.AsistenciasView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.AsistenciasVM {
    public class HomeAsistenciasVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand asistenciaCliente { get; set; }
        public RelayCommand asistenciaInstructor { get; set; }

        private AsistenciasHelper helper = new();

        private Asistencia asistencia = new();

        public Asistencia Asistencia {
            get { return asistencia; }
            set
            {
                asistencia = value;
                OnPropertyChanged(nameof(Asistencia));
            }
        }

        private int id;

        public int Id {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public HomeAsistenciasVM() {
            asistenciaCliente = new RelayCommand(AsistenciaClienteView);
            asistenciaInstructor = new RelayCommand(AsistenciaInstructorView);

        }

        private void AsistenciaInstructorView() {
            Asistencia.Tipo = 2;
            if (helper.CheckId(Asistencia)) {
                Asistencia = helper.CheckEntrada(Asistencia);
                AsistenciaInstructorWindow window = new(Asistencia);
                window.ShowDialog();
            }
        }

        private void AsistenciaClienteView() {
            Asistencia.Tipo = 1;
            if (helper.CheckId(Asistencia)) {
                Asistencia = helper.CheckEntrada(Asistencia);
                AsistenciaClienteWindow window = new(Asistencia);
                window.ShowDialog();
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

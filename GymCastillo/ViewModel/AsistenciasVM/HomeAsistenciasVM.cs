﻿using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.Helpers;
using GymCastillo.View.AsistenciasView;
using System.ComponentModel;

namespace GymCastillo.ViewModel.AsistenciasVM {
    public class HomeAsistenciasVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand asistenciaCliente { get; set; }
        public RelayCommand asistenciaInstructor { get; set; }

        private Asistencia asistencia = new();

        public Asistencia Asistencia {
            get { return asistencia; }
            set
            {
                asistencia = value;
                OnPropertyChanged(nameof(Asistencia));
            }
        }

        public HomeAsistenciasVM() {
            asistenciaCliente = new RelayCommand(AsistenciaClienteView);
            asistenciaInstructor = new RelayCommand(AsistenciaInstructorView);

        }

        private void AsistenciaInstructorView() {
            Asistencia.Tipo = 2;
            if (AsistenciasHelper.CheckId(Asistencia)) {
                Asistencia = AsistenciasHelper.CheckEntrada(Asistencia);
                AsistenciaInstructorWindow window = new(Asistencia);
                window.ShowDialog();
                Asistencia = null;
                Asistencia = new();
            }
        }

        public void AsistenciaClienteView() {
            Asistencia.Tipo = 1;
            if (AsistenciasHelper.CheckId(Asistencia)) {
                Asistencia = AsistenciasHelper.CheckEntrada(Asistencia);
                AsistenciaClienteWindow window = new(Asistencia);
                window.ShowDialog();
                Asistencia = null;
                Asistencia = new();
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

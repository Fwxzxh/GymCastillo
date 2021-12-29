using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.AsistenciasVM {
    public class AsistenciaInstructorVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand<IClosable> AsistenciaCommand { get; set; }
        public RelayCommand<IClosable> CloseWindowCommand { get; set; }

        private Asistencia asistencia;

        public Asistencia Asistencia {
            get { return asistencia; }
            set
            {
                asistencia = value;
                OnPropertyChanged(nameof(Asistencia));
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


        public AsistenciaInstructorVM(Asistencia asistencia) {
            this.asistencia = asistencia;
            instructor = asistencia.DatosInstructor;
            CloseWindowCommand = new RelayCommand<IClosable>(CloseWindow);
            AsistenciaCommand = new RelayCommand<IClosable>(RegistrarEntrada);

        }

        private async void RegistrarEntrada(IClosable obj) {
            Asistencia.SueldoADescontar = instructor.SueldoADescontar;
            await AsistenciasHelper.AsistenciaInstructor(Asistencia);
            obj.Close();
        }

        private void CloseWindow(IClosable obj) {
            if (obj != null) {
                obj.Close();
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

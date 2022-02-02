using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using System.ComponentModel;

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

        private decimal descuento;

        public decimal Descuento {
            get { return descuento; }
            set
            {
                descuento = value;
                OnPropertyChanged(nameof(Descuento));
            }
        }



        public AsistenciaInstructorVM(Asistencia asistencia) {
            this.asistencia = asistencia;
            instructor = asistencia.DatosInstructor;
            CloseWindowCommand = new RelayCommand<IClosable>(CloseWindow);
            AsistenciaCommand = new RelayCommand<IClosable>(RegistrarEntrada);

        }

        private async void RegistrarEntrada(IClosable obj) {
            instructor.SueldoADescontar += descuento;
            Asistencia.SueldoADescontar = instructor.SueldoADescontar;
            await AsistenciasHelper.AsistenciaInstructor(Asistencia);
            var lista = await GetFromDb.GetInstructores();
            InitInfo.ObCoInstructor.Clear();
            foreach (var item in lista) {
                InitInfo.ObCoInstructor.Add(item);
            }
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

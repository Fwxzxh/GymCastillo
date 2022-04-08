using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.PersonalScreensVM.InstructoresVM {
    public class InstructorClasesSummaryVM {
        public event PropertyChangedEventHandler PropertyChanged;

        public Instructor instructor { get; set; }

        private ObservableCollection<Clase> horario;

        public ObservableCollection<Clase> Horario {
            get { return horario; }
            set
            {
                horario = value;
                OnPropertyChanged(nameof(Horario));
            }
        }

        private string nombre;

        public string Nombre {
            get { return nombre; }
            set
            {
                nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }


        public InstructorClasesSummaryVM(Instructor intructor) {
            this.instructor = intructor;
            Nombre = instructor.Nombre + " " + instructor.ApellidoPaterno;
            Horario = new ObservableCollection<Clase>();
            var lista = instructor.GetClasesInstructor();
            foreach (var item in lista) {
                
                Horario.Add(item);
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

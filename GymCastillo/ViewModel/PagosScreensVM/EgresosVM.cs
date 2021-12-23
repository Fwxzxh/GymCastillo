using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes.Movimientos;
using GymCastillo.Model.DataTypes.Personal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.PagosScreensVM {
    public class EgresosVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand PagoUsuario { get; set; }
        public RelayCommand PagoInstructor { get; set; }
        public RelayCommand PagoPersonal { get; set; }
        public RelayCommand PagoServicios { get; set; }
        public RelayCommand PagoOtros { get; set; }

        private Usuario usuario = new();

        public Usuario Usuario {
            get { return usuario; }
            set
            {
                usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        private Instructor instructor = new();

        public Instructor Instructor {
            get { return instructor; }
            set
            {
                instructor = value;
                OnPropertyChanged(nameof(Instructor));
            }
        }

        private Personal personal = new();

        public Personal Personal {
            get { return personal; }
            set
            {
                personal = value;
                OnPropertyChanged(nameof(Personal));
            }
        }

        private Egresos egresos = new();

        public Egresos Egresos {
            get { return egresos; }
            set
            {
                egresos = value;
                OnPropertyChanged(nameof(Egresos));
            }
        }

        private int item = -1;

        public int Item {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged(nameof(Item));
                ClearData();
            }
        }

        private void ClearData() {
            Usuario = null;
            Usuario = new();
            Instructor = null;
            Instructor = new();
            Personal = null;
            Personal = new();
            Egresos = null;
            Egresos = new();
        }

        public EgresosVM() {

        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

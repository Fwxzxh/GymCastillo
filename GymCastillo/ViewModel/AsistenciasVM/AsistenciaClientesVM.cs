using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Init;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.AsistenciasVM {
    public class AsistenciaClientesVM : INotifyPropertyChanged {

        private Cliente cliente = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public Cliente Cliente {
            get { return cliente; }
            set { cliente = value;
                OnPropertyChanged(nameof(Cliente));
            }
        }

        public AsistenciaClientesVM(Asistencia asistencia) {
            Cliente = asistencia.DatosCliente;            
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

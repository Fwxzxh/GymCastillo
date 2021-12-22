using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.AsistenciasVM {
    public class AsistenciaClientesVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        private Cliente cliente = new();

        public Cliente Cliente {
            get { return cliente; }
            set
            {
                cliente = value;
                OnPropertyChanged(nameof(Cliente));
            }
        }

        private Asistencia asistencia;

        public Asistencia Asistencia {
            get { return asistencia; }
            set
            {
                asistencia = value;
                OnPropertyChanged(nameof(Asistencia));
            }
        }

        private List<Horario> listaHorarios;

        public List<Horario> ListaHorarios {
            get { return listaHorarios; }
            set
            {
                listaHorarios = value;
                OnPropertyChanged(nameof(ListaHorarios));
            }
        }


        public AsistenciaClientesVM(Asistencia asistencia) {
            Log.Debug("Ventana Asistencia Clientes iniciada");
            this.asistencia = asistencia;
            Cliente = asistencia.DatosCliente;
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
            ListaHorarios = asistencia.ListaHorarios;

        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

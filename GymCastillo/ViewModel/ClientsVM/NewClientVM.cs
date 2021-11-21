using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.Commands.ClientsCommands;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GymCastillo.ViewModel.ClientsVM {
    public class NewClientVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public NewClientCommand newClientCommand { get; set; }

        private Cliente newCliente = new();

        public Cliente NewCliente {
            get { return newCliente; }
            set
            {
                newCliente = value;
                OnPropertyChanged(nameof(NewCliente));
                MessageBox.Show(newCliente.Nombre);
            }
        }

        private string nombre;

        public string Nombre {
            get { return nombre; }
            set { nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }


        public NewClientVM() {
            newCliente.FechaNacimiento = DateTime.Now;
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
            newClientCommand = new(this);
        }

        public void CrearCliente() {
            Log.Debug("Nuevo usuario creado");
            MessageBox.Show(NewCliente.Nombre);
            MessageBox.Show(NewCliente.ApellidoMaterno);
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

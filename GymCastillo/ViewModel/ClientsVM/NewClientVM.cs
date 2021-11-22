using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.Commands.ClientsCommands;
using log4net;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using GymCastillo.Model.Admin;

namespace GymCastillo.ViewModel.ClientsVM {
    public class NewClientVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public NewClientCommand newClientCommand { get; set; }

        private Cliente newCliente = new();

        public Cliente NewCliente {
            get { return newCliente; }
            set {
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
            // Log.Debug("Nuevo usuario creado");
            // MessageBox.Show(NewCliente.Nombre);
            // var testClient = new ClienteRenta() {
            //     Id = 1,
            //     Nombre = "test",
            //     ApellidoPaterno = "testpppppp",
            //     ApellidoMaterno = "testm",
            //
            //     Domicilio = "domomomomo",
            //     FechaNacimiento = DateTime.Now,
            //     Telefono = "0123456789",
            //     NombreContacto = "contacto",
            //
            //     TelefonoContacto = "9876543210",
            //     Foto = null,
            //     FechaUltimoPago = DateTime.Now,
            //     MontoUltimoPago = 40,
            //
            //     DeudaCliente = 100
            // };
            Task.Run(() => AdminUsuariosGeneral.Delete(NewCliente));
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

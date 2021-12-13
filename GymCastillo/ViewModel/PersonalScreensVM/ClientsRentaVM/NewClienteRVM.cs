using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientesRentaCommands;
using log4net;

namespace GymCastillo.ViewModel.PersonalScreensVM.ClientsRentaVM {
    public class NewClienteRVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        public AltaClienteRentaCommand altaCliente { get; set; }
        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        private ClienteRenta cliente = new() {
            FechaUltimoPago = DateTime.Now,
            FechaNacimiento = DateTime.Now
        };

        public ClienteRenta Cliente {
            get { return cliente; }
            set
            {
                cliente = value;
                OnPropertyChanged(nameof(Cliente));
            }
        }


        public NewClienteRVM() {
            try {
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                altaCliente = new(this);
                Log.Debug("Cliente renta ViewModel inicializado");
            }
            catch (Exception e) {

                ShowPrettyMessages.ErrorOk(e.Message, "Error");
            }

        }

        public async void NewCliente() {
            await AdminUsuariosGeneral.Alta(Cliente);
            Log.Debug("Nuevo Cliente de renta registrado");
            Cliente = new() {
                FechaUltimoPago = DateTime.Now,
                FechaNacimiento = DateTime.Now
            };

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

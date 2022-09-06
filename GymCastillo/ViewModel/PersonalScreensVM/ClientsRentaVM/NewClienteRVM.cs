using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.IntersectionTables;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientesRentaCommands;
using ImageMagick;
using log4net;
using Microsoft.Win32;

namespace GymCastillo.ViewModel.PersonalScreensVM.ClientsRentaVM {
    public class NewClienteRVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        public AltaClienteRentaCommand altaCliente { get; set; }
        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }
        public RelayCommand ImageCommand { get; set; }


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


        private string photoPath;

        public string PhotoPath {
            get { return photoPath; }
            set
            {
                photoPath = value;
                OnPropertyChanged(nameof(PhotoPath));
            }
        }
        public NewClienteRVM() {
            try {
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                ImageCommand = new RelayCommand(SelectPhoto);
                altaCliente = new(this);
                Log.Debug("Cliente renta ViewModel inicializado");
            }
            catch (Exception e) {

                ShowPrettyMessages.ErrorOk(e.Message, "Error");
            }

        }

        private void SelectPhoto() {
            OpenFileDialog dialog = new() {
                Filter = "Image files|*.png;*.jpg;*.jpeg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };
            if (dialog.ShowDialog() == true) {
                PhotoPath = dialog.FileName;
                var image = new MagickImage(PhotoPath);
                cliente.Foto = image;
            }
        }

        public async void NewCliente() {
            Cliente.Telefono ??= "";
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

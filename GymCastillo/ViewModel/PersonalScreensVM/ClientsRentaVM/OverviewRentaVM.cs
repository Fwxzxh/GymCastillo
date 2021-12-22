using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientesRentaCommands;
using ImageMagick;
using log4net;
using Microsoft.Win32;

namespace GymCastillo.ViewModel.PersonalScreensVM.ClientsRentaVM {
    public class OverviewRentaVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }
        public RelayCommand ImageCommand { get; set; }

        public UpdateClienteCommand updateCliente { get; set; }

        public ClienteRenta clienteHold { get; set; }

        private ClienteRenta cliente;

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

        public OverviewRentaVM(ClienteRenta cliente) {
            try {
                this.cliente = cliente;
                clienteHold = cliente;
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                ImageCommand = new RelayCommand(SelectPhoto);

                updateCliente = new(this);

            }
            catch (Exception e) {
                Log.Error(e.Message);
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

        private void CloseWindow(IClosable window) {

            if (window != null) {
                window.Close();
             }
        }

        public async void UpdateCR() {
            await AdminUsuariosGeneral.Update(Cliente);
            Log.Debug("Cliente Renta actualilzado");
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

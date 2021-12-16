using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientsCommands;
using ImageMagick;
using log4net;
using Microsoft.Win32;

namespace GymCastillo.ViewModel.PersonalScreensVM.ClientsVM {
    public class OverviewClientsVM : INotifyPropertyChanged {

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public ObservableCollection<Paquete> paquetesList { get; set; }
        public ObservableCollection<Tipo> usuarioList { get; set; }
        public ObservableCollection<string> medioList { get; set; }

        private ObservableCollection<Locker> lockerList;

        public ObservableCollection<Locker> LockersList {
            get { return lockerList; }
            set
            {
                lockerList = value;
                OnPropertyChanged(nameof(LockersList));
            }
        }

        public SaveClientCommand saveClient { get; set; }

        public RelayCommand CredencialCommand { get; private set; }
        public RelayCommand ImageCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private Cliente selectedClient;
        public Cliente SelectedClient {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                if (selectedClient != null) {
                    OnPropertyChanged(nameof(SelectedClient));
                }
            }
        }

        private bool lockerIsChecked;

        public bool LockerIsChecked {
            get { return lockerIsChecked; }
            set
            {
                lockerIsChecked = value;
                OnPropertyChanged(nameof(LockerIsChecked));
                ReloadLockers();
            }
        }

        private decimal descuento;

        public decimal Descuento {
            get { return descuento; }
            set
            {
                descuento = value;
                OnPropertyChanged(nameof(Descuento));
                UpdatePago(descuento);
            }
        }

        private decimal pago;

        public decimal Pago {
            get { return pago; }
            set
            {
                pago = value;
                OnPropertyChanged(nameof(Pago));
            }
        }

        private string photoPath;

        public string PhotoPath {
            get { return photoPath; }
            set { photoPath = value;
                OnPropertyChanged(nameof(photoPath));
            }
        }


        public OverviewClientsVM(Cliente cliente) {
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
            SelectedClient = cliente;
            saveClient = new(this);
            ImageCommand = new RelayCommand(SelectPhoto);
            CredencialCommand = new RelayCommand(GenerarCredencial);
            paquetesList = new ObservableCollection<Paquete>(InitInfo.ObCoDePaquetes);
            usuarioList = new ObservableCollection<Tipo>(InitInfo.ObCoTipoCliente);
            lockerList = new ObservableCollection<Locker>();
            medioList = new ObservableCollection<string> {
                    "Amigos",
                    "Redes Sociales",
                    "Publicidad",
                    "Otros"
                };
            LockerIsChecked = SelectedClient.IdLocker != 0 ? true : false;

            //descuento = selectedClient.Descuento;
            pago = selectedClient.DeudaCliente;
        }

        private void GenerarCredencial() {
            Log.Debug("Creando credencial");
            DigitalCard.DrawCard(SelectedClient);
        }

        private void SelectPhoto() {
            OpenFileDialog dialog = new() {
                Filter = "Image files|*.png;*.jpg;*.jpeg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };
            if (dialog.ShowDialog() == true) {
                PhotoPath = dialog.FileName;
                var image = new MagickImage(PhotoPath);
                selectedClient.Foto = image;
            }
        }

        //TODO: filtrar por lockers desocupados + actual locker del cliente
        private void ReloadLockers() {
            LockersList.Clear();
            var locker = InitInfo.ObCoLockers;
            foreach (var item in locker) {
                LockersList.Add(item);
            }

            if (lockerIsChecked) {
                Pago += 50;
            }
            else {
                //SelectedClient.IdLocker = ;
                Pago = selectedClient.DeudaCliente;
            }
        }

        //TODO: implementar bien la lógica del costo del locker
        private void UpdatePago(decimal deuda) {

            if (deuda.Equals(0m)) {
                Pago = selectedClient.DeudaCliente;
            }
            else {
                Pago = Math.Max(0, Pago - deuda);
            }
        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }

        /// <summary>
        /// Método para hacer update del cliente, solo llama al selected cliente y guardar, ya tiene todos los cambios
        /// </summary>
        public void UpdateClient() {
            Task.Run(() => AdminUsuariosGeneral.Update(SelectedClient));
            Log.Debug("Usuario modificado");
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

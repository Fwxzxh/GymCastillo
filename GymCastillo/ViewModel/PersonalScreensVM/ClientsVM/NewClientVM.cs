using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes.Otros;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientsCommands;
using ImageMagick;
using log4net;
using Microsoft.Win32;

namespace GymCastillo.ViewModel.PersonalScreensVM.ClientsVM {
    public class NewClientVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

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

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public RelayCommand ImageCommand { get; set; }

        public NewClientCommand newClientCommand { get; set; }

        private Cliente newCliente = new();

        public Cliente NewCliente {
            get { return newCliente; }
            set
            {
                newCliente = value;
                OnPropertyChanged(nameof(NewCliente));
            }
        }

        private bool lockerIsChecked = false;

        private string photoPath;

        public string PhotoPath {
            get { return photoPath; }
            set { photoPath = value;
                OnPropertyChanged(nameof(PhotoPath));
            }
        }

        public bool LockerIsChecked {
            get { return lockerIsChecked; }
            set
            {
                lockerIsChecked = value;
                OnPropertyChanged(nameof(LockerIsChecked));
                ReloadLockers();
            }
        }

        private void ReloadLockers() {
            lockerList.Clear();
            var locker = InitInfo.ObCoLockersOpen;
            foreach (var item in locker) {
                lockerList.Add(item);
            }
        }

        public NewClientVM() {
            try {
                paquetesList = new ObservableCollection<Paquete>(InitInfo.ObCoDePaquetes);
                usuarioList = new ObservableCollection<Tipo>(InitInfo.ObCoTipoCliente);
                lockerList = new ObservableCollection<Locker>();
                medioList = new ObservableCollection<string> {
                    "Amigos",
                    "Redes Sociales",
                    "Publicidad",
                    "Otros"
                };
                ImageCommand = new RelayCommand(SelectPhoto);
                newCliente.FechaNacimiento = DateTime.Now;
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                newClientCommand = new(this);
                Log.Debug("Nuevo cliente ventana inicializada");
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
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
                newCliente.Foto = image;
            }
        }

        public async void CrearCliente() {
            Log.Debug("Nuevo usuario creado");
            await AdminUsuariosGeneral.Alta(NewCliente);
            NewCliente = new Cliente();

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

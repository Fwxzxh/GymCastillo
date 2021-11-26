using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.AdminScreensCommands.ClientsCommands;
using log4net;

namespace GymCastillo.ViewModel.AdminScreensVM.ClientsVM {
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
            var locker = InitInfo.ListaLockersOpen;
            foreach (var item in locker) {
                lockerList.Add(item);
            }
        }

        public NewClientVM() {
            try {
                paquetesList = new ObservableCollection<Paquete>(InitInfo.ListaDePaquetes);
                usuarioList = new ObservableCollection<Tipo>(InitInfo.ListaTipoCliente);
                lockerList = new ObservableCollection<Locker>();
                medioList = new ObservableCollection<string> {
                    "Amigos",
                    "Redes Sociales",
                    "Publicidad",
                    "Otros"
                };

                newCliente.FechaNacimiento = DateTime.Now;
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                newClientCommand = new(this);
                Log.Debug("Nuevo cliente ventana inicializada");
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
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

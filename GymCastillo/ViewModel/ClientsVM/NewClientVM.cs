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
using System.Collections.Generic;
using GymCastillo.Model.Database;
using System.Collections.ObjectModel;

namespace GymCastillo.ViewModel.ClientsVM {
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

        private bool lockerIsChecked;

        public bool LockerIsChecked {
            get { return lockerIsChecked; }
            set
            {
                lockerIsChecked = value;
                OnPropertyChanged(nameof(LockerIsChecked));
                ReloadLockers(lockerIsChecked);
            }
        }
    
        private void ReloadLockers(bool lockerIsChecked) {
            lockerList.Clear();
            var locker = Task.Run(() => GetFromDb.GetLockers(lockerIsChecked)).Result;
            foreach (var item in locker) {
                lockerList.Add(item);
            }
        }

        public NewClientVM() {
            try {
                paquetesList = new ObservableCollection<Paquete>(Task.Run(() => GetFromDb.GetPaquetes()).Result);
                usuarioList = new ObservableCollection<Tipo>(Task.Run(() => GetFromDb.GetTipoCliente()).Result);
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

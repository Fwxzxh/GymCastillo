using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.Commands.ClientsCommands;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GymCastillo.Model.Init;

namespace GymCastillo.ViewModel.ClientsVM {
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

        public event PropertyChangedEventHandler PropertyChanged;

        private Cliente selectedClient;
        public Cliente SelectedClient {
            get { return selectedClient; }
            set {
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

        public OverviewClientsVM(Cliente cliente) {
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
            SelectedClient = cliente;
            saveClient = new(this);

            paquetesList = new ObservableCollection<Paquete>(InitInfo.ListaDePaquetes);
            usuarioList = new ObservableCollection<Tipo>(InitInfo.ListaTipoCliente);
            lockerList = new ObservableCollection<Locker>();
            medioList = new ObservableCollection<string> {
                    "Amigos",
                    "Redes Sociales",
                    "Publicidad",
                    "Otros"
                };

        }

        private void ReloadLockers() {
            lockerList.Clear();
            var locker = InitInfo.ListaLockersOpen;
            foreach (var item in locker) {
                lockerList.Add(item);
            }
        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }

        /// <summary>
        /// Metodo para hacer update del cliente, solo llama al selected cliente y guardar, ya tiene todos los cambios
        /// </summary>
        public void UpdateClient() {
            Task.Run(() => AdminUsuariosGeneral.Update(selectedClient));
            Log.Debug("Usuario modificado");
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

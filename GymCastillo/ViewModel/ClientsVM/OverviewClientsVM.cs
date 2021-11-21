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
    public class OverviewClientsVM : INotifyPropertyChanged {

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public SaveClientCommand saveClient { get; set; }

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

        public OverviewClientsVM(Cliente cliente) {
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
            SelectedClient = cliente;
            saveClient = new(this);

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
            Log.Debug("Usuario modificado");
            MessageBox.Show(selectedClient.Telefono);
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

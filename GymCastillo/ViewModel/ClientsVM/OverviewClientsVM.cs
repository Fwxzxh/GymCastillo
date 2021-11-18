using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GymCastillo.ViewModel.ClientsVM {
    public class OverviewClientsVM : INotifyPropertyChanged {

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public SaveClientCommand saveClient { get; set; }

        public OverviewClientsVM(Cliente cliente) {
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
            SelectedClient = cliente;
            saveClient = new(this);

        }

        private void CloseWindow(IClosable window) {
            //var window = obj as Window;
            if (window != null) {
                window.Close();
            }
        }

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

        
        /// <summary>
        /// Metodo para hacer update del cliente, solo llama al selected cliente y guardar, ya tiene todos los cambios
        /// </summary>
        public void UpdateClient() {
            MessageBox.Show(selectedClient.Telefono);
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

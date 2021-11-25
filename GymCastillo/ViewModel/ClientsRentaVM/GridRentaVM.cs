using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.ClientsRentaVM {
    public class GridRentaVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        private List<ClienteRenta> clienteRenta { get; set; }

        public ObservableCollection<ClienteRenta> ListaClientes { get; set; }

        public GridRentaVM() {
            try {
                ListaClientes = new ObservableCollection<ClienteRenta>(InitInfo.ListaClientesRenta);
                clienteRenta = InitInfo.ListaClientesRenta;
                Log.Debug("Inicializada viewmodel grid rentas");
            }
            catch (Exception) {

                throw;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}

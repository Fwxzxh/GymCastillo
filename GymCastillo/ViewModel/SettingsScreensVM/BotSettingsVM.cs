using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.SettingsScreensVM {
    public class BotSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Cliente> clientes;

        public ObservableCollection<Cliente> Clientes {
            get { return clientes; }
            set
            {
                clientes = value;
                OnPropertyChanged(nameof(Clientes));
            }
        }

        public BotSettingsVM() {
            Clientes = new ObservableCollection<Cliente>();
            RefreshData();
        }

        private async void RefreshData() {
            var lista = await GetFromDb.GetClientes();
            //foreach (var cliente in lista.Where(c => c.ChatId != null || c.ChatId != "")) {
            //    clientes.Add(cliente);
            //}
            foreach (var cliente in lista) {
                Clientes.Add(cliente);
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

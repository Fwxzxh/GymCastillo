using GymCastillo.Model.DataTypes.Settings;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.SettingsScreensVM.SettingsVM {
    public class MainSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Espacio> ListaEspacios { get; set; }
        private Espacio espacio = new();

        public Espacio Espacio {
            get { return espacio; }
            set { espacio = value;
                OnPropertyChanged(nameof(espacio));
            }
        }

        public MainSettingsVM() {
            try {
                Log.Debug("Iniciada ventana de settings");
                ListaEspacios = new ObservableCollection<Espacio>(InitInfo.ListEspacios);
            }
            catch (Exception e) {
                Log.Error($"Error ventana de settings {e.Message}");
            }
            
        }
    }
}

using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.SettingsScreensVM.ClasesVM {
    public class ClasesSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public ObservableCollection<Clase> ListaClases { get; set; }

        private Clase clase;

        public Clase Clase {
            get { return clase; }
            set { clase = value;
                OnPropertyChanged(nameof(clase));
            }
        }

        public ClasesSettingsVM() {
            try {
                Log.Debug("Iniciando settings de clases.");
                ListaClases = new ObservableCollection<Clase>(InitInfo.ListaClases);
            }
            catch (Exception e) {
                Log.Error(e.Message);
                ShowPrettyMessages.ErrorOk(e.Message, "Error");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

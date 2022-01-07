using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Notificaciones;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.SettingsScreensVM {
    public class MainSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand ManualCommand { get; set; }
        public RelayCommand SaveKey { get; set; }

        public MainSettingsVM() {
            ManualCommand = new(Actualizar);
        }


        private async void Actualizar() {
            await Notificaciones.ResetFieldsAndUpdate();
            Notificaciones.FechaUltimoReset = DateTime.Now;
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

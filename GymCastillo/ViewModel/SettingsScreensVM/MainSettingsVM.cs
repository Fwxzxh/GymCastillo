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


        private string apiKey;

        public string ApiKey {
            get { return apiKey; }
            set
            {
                apiKey = value;
                OnPropertyChanged(nameof(ApiKey));
            }
        }

        public MainSettingsVM() {
            ApiKey = GetInitData.GetApiKey();
            ManualCommand = new(Actualizar);
            SaveKey = new RelayCommand(GuardarKey);
        }

        private void GuardarKey() {
            if (!string.IsNullOrWhiteSpace(ApiKey)) {
                GetInitData.WriteApikey(ApiKey);
            }
            else {
                ShowPrettyMessages.ErrorOk("La key no debe de estar vacía", "Error");
            }
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

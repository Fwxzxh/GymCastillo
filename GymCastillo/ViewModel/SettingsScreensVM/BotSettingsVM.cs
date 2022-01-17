using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Bot;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using log4net;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GymCastillo.ViewModel.SettingsScreensVM {
    public class BotSettingsVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        public event PropertyChangedEventHandler PropertyChanged;


        public RelayCommand StartBot { get; set; }
        public RelayCommand StopBot { get; set; }
        public RelayCommand GenPassword { get; set; }

        private Bot bot;

        public Bot Bot {
            get { return bot; }
            set
            {
                bot = value;
                OnPropertyChanged(nameof(Bot));
            }
        }

        private string password;

        public string Password {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private bool estado;

        public bool Estado {
            get { return estado; }
            set
            {
                estado = value;
                OnPropertyChanged(nameof(Estado));
            }
        }

        public BotSettingsVM() {
            GetInitData.GetApiKey();
            StartBot = new RelayCommand(IniciarBot);
            StopBot = new RelayCommand(DetenerBot);
            GenPassword = new RelayCommand(GenerarPassword);
        }

        private async void GenerarPassword() {
            Password = Bot.GenPassword();
            InitInfo.ObCoClientes.Clear();
            var lista = await GetFromDb.GetClientes();
            foreach (var item in lista) {
                InitInfo.ObCoClientes.Add(item);
            }
        }

        private void DetenerBot() {
            Bot.StopBot();
            Estado = Bot.Estado;
        }

        private void IniciarBot() {
            var key = GetInitData.GetApiKey();
            Bot = new Bot(key);
            Estado = Bot.Estado;
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

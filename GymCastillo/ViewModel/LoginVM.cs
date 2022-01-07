using log4net;
using MySqlConnector;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GymCastillo.Model.Bot;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.Model.Notificaciones;

namespace GymCastillo.ViewModel {
    public class LoginVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public Action CloseAction { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginCommand loginCommand { get; set; }

        private string userName;

        public string UserName {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        private string password;

        public string Password {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public Action Close { get; set; }

        public LoginVM() {
            loginCommand = new LoginCommand();
        }

        public async void LogIn(string userName, string password) {

            try {
                if (Init.LogIn(userName, password)) {

                    // Cargamos la ventana principal
                    Log.Info("LogIn exitoso.");
                    var init= new InitInfo(); // Obtenemos la información inicial
                    if (init.DoneTasks) {
                        await Notificaciones.CheckResetFields(); // Verificamos si debemos resetear los cupos.

                        // iniciamos el bot.
                        GetInitData.WriteApikey("5031509807:AAEqBUEnXaARUzeFAWjd-Tk_FQt220LyEfM");
                        var key = GetInitData.GetApiKey();
                        // var bot = new Bot("5031509807:AAEqBUEnXaARUzeFAWjd-Tk_FQt220LyEfM");
                        var bot = new Bot(key);
                        // await Bot.SendMessage("aaaa \n aaaaa \n faaaaa", 5);
                        // bot.StopBot();

                        var x = Bot.Estado;
                        MainWindow main = new();
                        main.Show();
                        Application.Current.MainWindow.Close();
                    }
                }
                else {
                    Log.Info("LogIn fallido, credenciales erróneas.");
                    ShowPrettyMessages.WarningOk(
                        "Usuario y/o contraseña incorrectos.",
                        "Error de credenciales");
                }
            }
            catch (MySqlException e) {
                Log.Error("El string de conexión probablemente sea erróneo.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.WarningOk(
                    "Error: verifica tus credenciales de base de datos, probablemente sean erróneas.",
                    "Error de conexión");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error en el proceso de login.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk($"Error: {e.Message}", "Error desconocido en LogIn");
            }
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class Converter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            return null;
        }
    }
}

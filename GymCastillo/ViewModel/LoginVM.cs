using log4net;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;

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

        public void LogIn(string userName, string password) {
            try {
                if (Init.LogIn(userName, password)) {

                    // Cargamos la ventana principal
                    Log.Info("LogIn exitoso.");
                    var done = Task.Run(() => InitInfo.GetAllInfo());
                    if (done.GetAwaiter().GetResult()) {
                        MainWindow main = new();
                        main.Show();
                        Application.Current.MainWindow.Close();
                    }
                }
                else {
                    Log.Info("LogIn fallido, credenciales erroneas.");
                    ShowPrettyMessages.WarningOk(
                        "Usuario y/o contraseña incorrectos.",
                        "Error de credenciales");
                }
            }
            catch (MySqlException e) {
                Log.Error("El string de conexión probablemente sea erroneo.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.WarningOk(
                    "Error: verifica tus credenciales de base de datos, probablemente sean erroneas.",
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

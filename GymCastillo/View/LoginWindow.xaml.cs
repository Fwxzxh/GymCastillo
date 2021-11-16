using System;
using System.Windows;
using GymCastillo.ViewModel.Helpers;
using GymCastillo.ViewModel.Init;
using log4net;
using MySqlConnector;

namespace GymCastillo {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public LoginWindow() {
            InitializeComponent();
            Log.Debug("Se ha inicializado con éxito la pantalla de LogIn");
        }

        /// <summary>
        /// Evento del boton de LogIn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginBtnClick(object sender, RoutedEventArgs e) {
            Log.Debug("Se ha precionado el botón de logIn.");
            LogIn();
        }

        /// <summary>
        /// Método que se encarga de el proceso de logIn
        /// </summary>
        private void LogIn() {
            try {
                if (Init.LogIn(txtUsuario.Text, txtPassword.Password)) {

                    // Cargamos la ventana principal
                    Log.Info("LogIn exitoso.");
                    MainWindow main = new();
                    main.Show();
                    Close();
                }
                else {
                    Log.Info("LogIn fallido, credenciales erroneas.");
                    ShowPrettyMessages.WarningOk(
                        "Usuario y/o contraseña incorrectos.",
                        "Error de credenciales");
                }
            }
            catch (MySqlException e) {
                Log.Error("El string de connexión probablemente sea erroneo.");
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

        private void txtUsuario_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter) {
                txtPassword.Focus();
            }
        }

        private void txtPassword_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter) {
                LogIn();
            }
        }
    }
}

using System;
using System.Windows;
using GymCastillo.Model.Init;
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
                    MessageBox.Show("Usuario y/o contraseña erroneos.");
                }
            }

            catch (MySqlException e) {
                Log.Error("Se ha intentado ");
                MessageBox.Show("Tus datos de conección son erroneos");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error en el proceso de logIn.");
                Log.Error($"Error: {e.Message}");
                // TODO: ver como manejar el error
            }
        }
    }
}

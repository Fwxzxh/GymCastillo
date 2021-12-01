using System;
using System.Windows;
using GymCastillo.ViewModel;
using log4net;
using MySqlConnector;

namespace GymCastillo {

    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public LoginWindow() {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
            Log.Debug("Se ha inicializado con éxito la pantalla de LogIn");
        }
    }
}

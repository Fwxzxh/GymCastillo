using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Init;

namespace GymCastillo {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window {
        public LoginWindow() {
            InitializeComponent();
        }

        private void LoginBtnClick(object sender, RoutedEventArgs e) {
            MainWindow main = new();
            main.Show();
            Close();

            // test login
            if (Init.LogIn("admin", "admin")) {
                MessageBox.Show("Login ok");
            }
        }
    }
}

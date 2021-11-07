using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GymCastillo.Model.Database;
using GymCastillo.Model.Helpers;
using log4net;

namespace GymCastillo.View {
    /// <summary>
    /// Interaction logic for GridClientesUserControl.xaml
    /// </summary>
    public partial class GridClientesUserControl : UserControl {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public GridClientesUserControl() {
            try {
                InitializeComponent();
                ClientesDataGrid.ItemsSource = GetFromDb.GetClientes().Result;

                Log.Debug("Se ha inicializado y se han obtenido los datos de la pantalla de GridClientes.");
            }
            catch (Exception e) {
                Log.Error("Ha ocurrido un error al iniciar la pantalla de GridClientes.");
                Log.Error($"Error: {e.Message}");
                ShowPrettyMessages.ErrorOk(
                    $"Ha ocurrido un error desconocido al iniciar la pantalla de GridClientes. Error: {e.Message}",
                    "Error Desconocido");
            }
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e) {
            NewClientsWindow newClients = new();
            newClients.ShowDialog();
        }   
    }
}

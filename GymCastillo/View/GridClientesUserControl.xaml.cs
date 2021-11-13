using System;
using System.Windows;
using System.Windows.Controls;
using GymCastillo.ViewModel.Database;
using GymCastillo.ViewModel.Helpers;
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

﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.IntersectionTables;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.View.PersonalScreenView.ClientsRentaView;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientesRentaCommands;
using log4net;

namespace GymCastillo.ViewModel.PersonalScreensVM.ClientsRentaVM {
    public class GridRentaVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        private ObservableCollection<ClienteRenta> clienteRenta { get; set; }

        public NewCRWindowCommand newCRCommand { get; set; }

        public OverviewCRCommand overview { get; set; }

        public DeleteCRCommand delete { get; set; }

        public ObservableCollection<ClienteRenta> ListaClientes { get; set; }

        private string query = "";

        public string Query {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged(nameof(Query));
                FilterData(value);
            }
        }

        private static void FilterData(string value) {
            if (value != null) {
                CollectionViewSource.GetDefaultView(InitInfo.ObCoClientesRenta).Filter = item => (item as ClienteRenta).Nombre.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
            }
            else CollectionViewSource.GetDefaultView(InitInfo.ObCoClientesRenta);
        }

        private ClienteRenta selectedCliente;

        public ClienteRenta SelectedCliente {
            get { return selectedCliente; }
            set
            {
                selectedCliente = value;
                OnPropertyChanged(nameof(SelectedCliente));
            }
        }


        public GridRentaVM() {
            try {
                //var collectionView = CollectionViewSource.GetDefaultView(InitTest.ObCoClienteRenta);

                //ListaClientes = new ObservableCollection<ClienteRenta>(InitInfo.ObCoClientesRenta);
                //clienteRenta = InitInfo.ObCoClientesRenta;
                newCRCommand = new(this);
                overview = new(this);
                delete = new(this);
                Log.Debug("Inicializada viewmodel grid rentas");
            }
            catch (Exception e) {
                Log.Error(e.Message);

            }
        }

        public void OpenOverviewCR() {
            OverviewCRWindow overviewCR = new(selectedCliente);
            overviewCR.ShowDialog();
            RefreshGrid();
        }

        public void OpenNewCR() {
            NewCRWindow newCR = new();
            newCR.ShowDialog();
            RefreshGrid();
        }

        public async void DeleteCR() {
            if (ShowPrettyMessages.QuestionYesNo("¿Desea borrar al cliente?", "Confirmación")) {
                await AdminUsuariosGeneral.Delete(SelectedCliente);
                RefreshGrid();
            }
            else return;
        }

        private async void RefreshGrid() {
            //ListaClientes.Clear();
            InitInfo.ObCoClientesRenta.Clear();
            var usuarios = await GetFromDb.GetClientesRenta();
            foreach (var item in usuarios.OrderBy(c => c.Nombre)) {
                InitInfo.ObCoClientesRenta.Add(item);
            }
            //InitInfo.ListaClientesRenta = usuarios;
        }

        private async void FilterList(string query) {
            clienteRenta = await GetFromDb.GetClientesRenta();
            if (clienteRenta != null) {
                if (string.IsNullOrWhiteSpace(query)) {
                    ListaClientes.Clear();
                    foreach (var cliente in clienteRenta.OrderBy(i => i.Nombre)) {
                        ListaClientes.Add(cliente);
                    }
                }
                else {
                    ListaClientes.Clear();
                    var filteredList = clienteRenta.Where(c => c.Nombre.ToLower().Contains(query.ToLower()) || c.ApellidoPaterno.ToLower().Contains(query.ToLower()) || c.ApellidoMaterno.ToLower().Contains(query.ToLower())).ToList().OrderBy(c => c.Nombre);
                    foreach (var cliente in filteredList) {
                        ListaClientes.Add(cliente);
                    }
                }
            }
            else return;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}

﻿using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Admin;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.Commands.ClientesRentaCommands;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.ClientsRentaVM {
    public class OverviewRentaVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public UpdateClienteCommand updateCliente { get; set; }


        private ClienteRenta cliente;

        public ClienteRenta Cliente {
            get { return cliente; }
            set
            {
                cliente = value;
                OnPropertyChanged(nameof(Cliente));
            }
        }

        public OverviewRentaVM(ClienteRenta cliente) {
            try {
                this.cliente = cliente;
                CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
                updateCliente = new(this);

            }
            catch (Exception) {

                throw;
            }
        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }

        public async void UpdateCR() {
            await AdminUsuariosGeneral.Update(Cliente);
            Log.Debug("Cliente Renta actualilzado");
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

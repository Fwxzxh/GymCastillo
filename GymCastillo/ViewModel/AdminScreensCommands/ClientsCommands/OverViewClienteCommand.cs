﻿using System;
using System.Windows.Input;
using GymCastillo.ViewModel.AdminScreensVM.ClientsVM;

namespace GymCastillo.ViewModel.AdminScreensCommands.ClientsCommands {
    public class OverViewClienteCommand : ICommand {

        public GridClientesVM ClientesVM { get; set;}
        public OverViewClienteCommand(GridClientesVM clientesVM) {
            ClientesVM = clientesVM;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            ClientesVM.OpenOverview();  
        }
    }
}

﻿using System;
using System.Windows.Input;
using GymCastillo.ViewModel.PersonalScreensVM.ClientsRentaVM;

namespace GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientesRentaCommands {
    public class AltaClienteRentaCommand : ICommand {
        private NewClienteRVM vm { get; set; }
        public AltaClienteRentaCommand(NewClienteRVM vm) {
            this.vm = vm;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            vm.NewCliente();
        }

    }
}

﻿using System;
using System.Windows.Input;
using GymCastillo.ViewModel.AdminScreensVM.ClientsRentaVM;

namespace GymCastillo.ViewModel.AdminScreensCommands.ClientesRentaCommands {
    public class DeleteCRCommand : ICommand {
        private GridRentaVM vm { get;set; }
        public DeleteCRCommand(GridRentaVM vm ) {
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
            vm.DeleteCR();
        }
    }
}

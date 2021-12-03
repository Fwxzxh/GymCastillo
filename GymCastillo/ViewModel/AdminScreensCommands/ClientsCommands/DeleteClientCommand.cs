﻿using System;
using System.Windows.Input;
using GymCastillo.ViewModel.AdminScreensVM.ClientsVM;

namespace GymCastillo.ViewModel.AdminScreensCommands.ClientsCommands {
    public class DeleteClientCommand : ICommand {
 
        private GridClientesVM vm { get; set; }

        public DeleteClientCommand(GridClientesVM vm) {
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
            vm.DeleteClient();
        }
    }
}
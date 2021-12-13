using System;
using System.Windows.Input;
using GymCastillo.ViewModel.PersonalScreensVM.ClientsRentaVM;

namespace GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientesRentaCommands {
    public class UpdateClienteCommand : ICommand {
        private OverviewRentaVM vm { get; set; }
        public UpdateClienteCommand(OverviewRentaVM vm) {
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
            vm.UpdateCR();
        }
    }
}

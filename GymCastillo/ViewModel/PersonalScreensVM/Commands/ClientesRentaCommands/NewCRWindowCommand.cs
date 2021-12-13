using System;
using System.Windows.Input;
using GymCastillo.ViewModel.PersonalScreensVM.ClientsRentaVM;

namespace GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientesRentaCommands {
    public class NewCRWindowCommand : ICommand {
        private GridRentaVM vm { get; set; }
        public NewCRWindowCommand(GridRentaVM vm) {
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
            vm.OpenNewCR();
        }
    }
}

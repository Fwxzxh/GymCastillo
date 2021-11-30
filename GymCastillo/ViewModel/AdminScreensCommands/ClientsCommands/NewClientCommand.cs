using System;
using System.Windows.Input;
using GymCastillo.ViewModel.AdminScreensVM.ClientsVM;

namespace GymCastillo.ViewModel.AdminScreensCommands.ClientsCommands {
    public class NewClientCommand : ICommand {

        private NewClientVM vm { get; set; }

        public NewClientCommand( NewClientVM vM) {
            vm = vM;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            vm.CrearCliente();
        }
    }
}

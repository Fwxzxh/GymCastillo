using GymCastillo.ViewModel.ClientsRentaVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GymCastillo.ViewModel.Commands.ClientesRentaCommands {
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

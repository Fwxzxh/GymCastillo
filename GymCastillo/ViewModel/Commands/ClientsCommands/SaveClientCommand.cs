using GymCastillo.ViewModel.ClientsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GymCastillo.ViewModel.Commands.ClientsCommands {
    public class SaveClientCommand : ICommand {
        public OverviewClientsVM vm { get; set; }

        public SaveClientCommand(OverviewClientsVM clientsVM) {
            vm = clientsVM;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            vm.UpdateClient();
        }
    }
}

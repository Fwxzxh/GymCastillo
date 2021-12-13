using System;
using System.Windows.Input;
using GymCastillo.ViewModel.PersonalScreensVM.ClientsVM;

namespace GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientsCommands {
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

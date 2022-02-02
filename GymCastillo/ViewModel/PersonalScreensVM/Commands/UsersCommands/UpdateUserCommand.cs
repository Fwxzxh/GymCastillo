using System;
using System.Windows.Input;
using GymCastillo.ViewModel.AdminScreensVM.UsersVM;

namespace GymCastillo.ViewModel.PersonalScreensVM.Commands.UsersCommands {
    public class UpdateUserCommand : ICommand {
        private OverviewUsuariosVM vm { get; set; }

        public UpdateUserCommand(OverviewUsuariosVM vm) {
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
            vm.UpdateUser();
        }
    }
}

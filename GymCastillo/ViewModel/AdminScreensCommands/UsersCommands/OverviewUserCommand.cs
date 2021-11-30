using System;
using System.Windows.Input;
using GymCastillo.ViewModel.AdminScreensVM.UsersVM;

namespace GymCastillo.ViewModel.AdminScreensCommands.UsersCommands {
    public class OverviewUserCommand : ICommand {

        private GridUsuariosVM vm { get; set; }
        public OverviewUserCommand(GridUsuariosVM vm) {
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
            vm.OpenOverview();
        }
    }
}

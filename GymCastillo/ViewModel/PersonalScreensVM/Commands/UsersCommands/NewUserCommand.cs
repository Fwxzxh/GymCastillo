using System;
using System.Windows.Input;
using GymCastillo.ViewModel.AdminScreensVM.UsersVM;

namespace GymCastillo.ViewModel.PersonalScreensVM.Commands.UsersCommands {
    public class NewUserCommand : ICommand {
        private NewUsuarioVM vm { get; set; }
        public NewUserCommand(NewUsuarioVM vM) {
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
            vm.NewUser();
        }
    }
}

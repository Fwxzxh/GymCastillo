using System;
using System.Windows.Input;
using GymCastillo.ViewModel.AdminScreensVM.UsersVM;

namespace GymCastillo.ViewModel.PersonalScreensVM.Commands.UsersCommands {
    public class DeleteUserCommand : ICommand {
        private GridUsuariosVM vM { get; set; }

        public DeleteUserCommand(GridUsuariosVM vM) {
            this.vM = vM;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            vM.DeleteUsuario();
        }
    }
}

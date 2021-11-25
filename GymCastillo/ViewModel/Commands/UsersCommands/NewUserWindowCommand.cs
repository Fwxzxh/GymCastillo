using GymCastillo.ViewModel.UsersVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GymCastillo.ViewModel.Commands.UsersCommands {
    public class NewUserWindowCommand : ICommand {

        private GridUsuariosVM vM { get; set; }
        public NewUserWindowCommand(GridUsuariosVM vM) {
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
            vM.OpenNewUser();
        }
    }
}

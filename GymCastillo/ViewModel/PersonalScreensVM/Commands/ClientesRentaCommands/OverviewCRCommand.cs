using System;
using System.Windows.Input;
using GymCastillo.ViewModel.PersonalScreensVM.ClientsRentaVM;

namespace GymCastillo.ViewModel.PersonalScreensVM.Commands.ClientesRentaCommands {
    public class OverviewCRCommand : ICommand {
        private GridRentaVM vM { get; set; }

        public OverviewCRCommand(GridRentaVM vM) {
            this.vM=vM;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            vM.OpenOverviewCR();
        }
    }
}

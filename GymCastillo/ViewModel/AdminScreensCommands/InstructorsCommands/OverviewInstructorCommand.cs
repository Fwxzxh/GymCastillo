using System;
using System.Windows.Input;
using GymCastillo.ViewModel.AdminScreensVM.InstructoresVM;

namespace GymCastillo.ViewModel.AdminScreensCommands.InstructorsCommands {
    public class OverviewInstructorCommand : ICommand {

        private GridInstructoresVM vM { get; set; }

        public OverviewInstructorCommand(GridInstructoresVM vm) {
            vM = vm;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            vM.OpenOverview();
        }
    }
}

using System;
using System.Windows.Input;
using GymCastillo.ViewModel.PersonalScreensVM.InstructoresVM;

namespace GymCastillo.ViewModel.PersonalScreensVM.Commands.InstructorsCommands {
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

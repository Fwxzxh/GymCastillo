using System;
using System.Windows.Input;
using GymCastillo.ViewModel.AdminScreensVM.InstructoresVM;

namespace GymCastillo.ViewModel.AdminScreensCommands.InstructorsCommands {
    public class NewWindowInstructorCommand : ICommand {
        private GridInstructoresVM window { get; set; }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public NewWindowInstructorCommand(GridInstructoresVM vM) {
            window = vM;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            window.OpenWindow();
        }
    }
}

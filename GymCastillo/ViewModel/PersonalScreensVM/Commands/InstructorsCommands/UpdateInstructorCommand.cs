using System;
using System.Windows.Input;
using GymCastillo.ViewModel.PersonalScreensVM.InstructoresVM;

namespace GymCastillo.ViewModel.PersonalScreensVM.Commands.InstructorsCommands {
    public class UpdateInstructorCommand : ICommand {

        private OverviewInstructorVM vm { get; set; }

        public UpdateInstructorCommand(OverviewInstructorVM VM) {
            vm = VM;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            vm.UpdateInstructor();
        }
    }
}

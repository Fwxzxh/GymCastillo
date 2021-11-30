using System;
using System.Windows.Input;
using GymCastillo.ViewModel.AdminScreensVM.InstructoresVM;

namespace GymCastillo.ViewModel.AdminScreensCommands.InstructorsCommands {
    public class DeleteInstructorCommand : ICommand {
        private GridInstructoresVM vm { get; set; }

        public DeleteInstructorCommand(GridInstructoresVM vm) {
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
            vm.DeleteInstructor();
        }
    }
}

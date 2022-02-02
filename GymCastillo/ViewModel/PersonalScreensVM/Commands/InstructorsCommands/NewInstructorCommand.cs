using System;
using System.Windows.Input;
using GymCastillo.ViewModel.PersonalScreensVM.InstructoresVM;

namespace GymCastillo.ViewModel.PersonalScreensVM.Commands.InstructorsCommands {
    public class NewInstructorCommand : ICommand {

        private NewInstructorVM vm { get; set; }

        public NewInstructorCommand(NewInstructorVM vM) {
            this.vm = vM;

        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            vm.CrearInstructor();
        }
    }
}

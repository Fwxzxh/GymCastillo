using GymCastillo.ViewModel.InstructoresVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GymCastillo.ViewModel.Commands.InstructorsCommands {
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

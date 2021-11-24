using GymCastillo.View.ClientsView;
using GymCastillo.ViewModel.ClientsVM;
using GymCastillo.ViewModel.InstructoresVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GymCastillo.ViewModel.Commands.InstructorsCommands {
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

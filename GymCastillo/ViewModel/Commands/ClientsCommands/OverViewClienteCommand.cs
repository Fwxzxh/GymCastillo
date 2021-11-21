using GymCastillo.Model.DataTypes;
using GymCastillo.ViewModel.ClientsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GymCastillo.ViewModel.Commands.ClientsCommands {
    public class OverViewClienteCommand : ICommand {

        public GridClientesVM ClientesVM { get; set;}
        public OverViewClienteCommand(GridClientesVM clientesVM) {
            ClientesVM = clientesVM;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            ClientesVM.OpenOverview();  
        }
    }
}

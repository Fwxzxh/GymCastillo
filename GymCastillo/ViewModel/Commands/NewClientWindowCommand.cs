using GymCastillo.View.ClientsView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GymCastillo.ViewModel.Commands {

    public class NewClientWindowCommand : ICommand {
       

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        
        // Puede ejecutar
        public bool CanExecute(object parameter) {
            return true;
        }

        // Ejecuta todo lo de este metodo
        public void Execute(object parameter) {
            NewClientsWindow newClientsWindow = new();
            newClientsWindow.ShowDialog();
        }
    }
}

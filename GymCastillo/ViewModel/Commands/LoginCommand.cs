using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GymCastillo.ViewModel.Commands
{
    public class LoginCommand : ICommand{

        private LoginVM loginVM;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object parameter) {
            var values = (object[])parameter;
            if (values != null) {
                var username = values[0] == null ? "" : values[0].ToString();
                var passwordBox = values[1] as PasswordBox;
                var password = passwordBox.Password;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) {
                    return false;
                }
            }
            return true;
        }

        public void Execute(object parameter) {
            loginVM = new LoginVM();
            var values = (object[])parameter;
            var password = values[1] as PasswordBox;

            loginVM.LogIn(values[0].ToString(),password.Password);

        }
    }
}

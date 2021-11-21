using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.InstructoresVM {
    internal class NewInstructorVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }


        public NewInstructorVM() {
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);

        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }
    }
}

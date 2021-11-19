using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GymCastillo.ViewModel.ClientsVM {
    public class NewClientVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<IClosable> CloseWindowCommand { get; private set; }

        public NewClientCommand newClientCommand { get; set; }

        private Cliente newCliente = new();

        public Cliente NewCliente {
            get { return newCliente; }
            set
            {
                newCliente = value;
                OnPropertyChanged(nameof(NewCliente));
                MessageBox.Show(newCliente.Nombre);
            }
        }

        private string nombre;

        public string Nombre {
            get { return nombre; }
            set { nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }


        public NewClientVM() {
            newCliente.FechaNacimiento = DateTime.Now;
            CloseWindowCommand = new RelayCommand<IClosable>(this.CloseWindow);
            newClientCommand = new(this);
        }

        private void CloseWindow(IClosable window) {
            if (window != null) {
                window.Close();
            }
        }

        public void CrearCliente() {
            MessageBox.Show(NewCliente.Nombre);
            MessageBox.Show(NewCliente.ApellidoMaterno);
            //newCliente.Nombre
            //newCliente.ApellidoMaterno
            //aqui guardamos el nuevo cliente
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

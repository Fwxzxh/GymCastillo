using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Init;
using GymCastillo.View.UsuariosView;
using GymCastillo.ViewModel.Commands.UsersCommands;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymCastillo.ViewModel.UsersVM {
    public class GridUsuariosVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public NewUserWindowCommand newUserWindow { get; set; }

        public OverviewUserCommand overview { get; set; }

        public ObservableCollection<Usuario> ListaUsuarios { get; set; }

        private List<Usuario> usuarios;

        public List<Usuario> Usuarios {
            get { return usuarios; }
            set
            {
                usuarios = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        private Usuario selectedUsuario;

        public Usuario SelectedUsuario {
            get { return selectedUsuario; }
            set
            {
                selectedUsuario = value;
                OnPropertyChanged(nameof(SelectedUsuario));
            }
        }


        public GridUsuariosVM() {
            try {
                ListaUsuarios = new ObservableCollection<Usuario>(InitInfo.ListaUsuarios);
                usuarios = InitInfo.ListaUsuarios;

                newUserWindow = new(this);
                overview = new(this);


            }
            catch (Exception) {

                throw;
            }
        }

        public void OpenOverview() {
            OverviewUserWindow overviewUser = new(SelectedUsuario);
            overviewUser.ShowDialog();
        }

        public void OpenNewUser() {
            NewUserWindow newUser = new();
            newUser.ShowDialog();
        }


        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

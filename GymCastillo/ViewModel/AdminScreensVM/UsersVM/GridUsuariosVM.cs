using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.View.AdminScreensView.UsuariosView;
using GymCastillo.ViewModel.AdminScreensCommands.UsersCommands;
using log4net;

namespace GymCastillo.ViewModel.AdminScreensVM.UsersVM {
    public class GridUsuariosVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public event PropertyChangedEventHandler PropertyChanged;

        public NewUserWindowCommand newUserWindow { get; set; }

        public OverviewUserCommand overview { get; set; }

        public DeleteUserCommand delete { get; set; }

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

        private string query;

        public string Query {
            get { return query; }
            set { query = value;
                OnPropertyChanged(nameof(Query));
                FilterList(Query);
            }
        }


        public GridUsuariosVM() {
            try {
                ListaUsuarios = new ObservableCollection<Usuario>(InitInfo.ListaUsuarios);
                usuarios = InitInfo.ListaUsuarios;
                delete = new(this);
                newUserWindow = new(this);
                overview = new(this);
            }
            catch (Exception e) {
                Log.Error(e.Message);
                //ShowPrettyMessages.ErrorOk(e.Message, "Error");
                throw;
            }
        }

        private async void RefreshGrid() {
            ListaUsuarios.Clear();
            var usuarios = await GetFromDb.GetUsuarios();
            InitInfo.ListaUsuarios = usuarios;
            foreach (var item in usuarios.OrderBy(c => c.Nombre)) {
                ListaUsuarios.Add(item);
            }
        }

        public void OpenOverview() {
            OverviewUserWindow overviewUser = new(SelectedUsuario);
            overviewUser.ShowDialog();
            RefreshGrid();
        }

        public void OpenNewUser() {
            NewUserWindow newUser = new();
            newUser.ShowDialog();
            RefreshGrid();
        }

        public async void DeleteUsuario() {
            if (ShowPrettyMessages.QuestionYesNo("¿Desea borrar al usuario?", "Confirmación")) {
                await AdminUsuariosGeneral.Delete(SelectedUsuario);
                RefreshGrid();
            }
            else return;
        }

        private async void FilterList(string query) {
            usuarios = await GetFromDb.GetUsuarios(); 
            if (usuarios != null) {
                if (string.IsNullOrWhiteSpace(query)) {
                    ListaUsuarios.Clear();
                    foreach (var cliente in usuarios.OrderBy(i => i.Nombre)) {
                        ListaUsuarios.Add(cliente);
                    }
                }
                else {
                    ListaUsuarios.Clear();
                    var filteredList = usuarios.Where(c => c.Nombre.ToLower().Contains(query.ToLower()) || c.ApellidoPaterno.ToLower().Contains(query.ToLower()) || c.ApellidoMaterno.ToLower().Contains(query.ToLower())).ToList().OrderBy(c => c.Nombre);
                    foreach (var cliente in filteredList) {
                        ListaUsuarios.Add(cliente);
                    }
                }
            }
            else return;
        }


        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

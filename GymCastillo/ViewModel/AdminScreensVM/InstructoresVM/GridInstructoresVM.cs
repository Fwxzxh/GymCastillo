using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.View.PersonalScreenView.InstructoresView;
using GymCastillo.ViewModel.AdminScreensCommands.InstructorsCommands;
using log4net;

namespace GymCastillo.ViewModel.AdminScreensVM.InstructoresVM {
    public class GridInstructoresVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);


        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Instructor> ListaInstructores { get; set; }

        public NewWindowInstructorCommand newWindow { get; set; }

        public OverviewInstructorCommand overviewInstructor { get; set; }

        public DeleteInstructorCommand delete { get; set; }

        private List<Instructor> instructores { get; set; }

        private string query;

        public string Query {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged(nameof(Query));
                FilterList(query);
            }
        }

        private Instructor selectedInstructor;

        public Instructor SelectedInstructor {
            get { return selectedInstructor; }
            set
            {
                selectedInstructor = value;
                OnPropertyChanged(nameof(SelectedInstructor));
            }
        }

        public GridInstructoresVM() {
            try {
                newWindow = new(this);
                overviewInstructor = new(this);
                instructores = InitInfo.ListaInstructor;
                delete = new(this);
                ListaInstructores = new ObservableCollection<Instructor>();
                foreach (var item in instructores.OrderBy(i => i.Nombre)) {
                    ListaInstructores.Add(item);
                }

                Log.Debug("Inicializada la pantalla de instructores con éxito.");
            }
            catch (Exception e) {
                Log.Error(e.Message);
                //MessageBox.Show(e.Message);
            }


        }
        private async void FilterList(string query) {
            instructores = await GetFromDb.GetInstructores();
            if (instructores != null) {
                if (string.IsNullOrWhiteSpace(query)) {
                    ListaInstructores.Clear();
                    foreach (var cliente in instructores.OrderBy(i => i.Nombre)) {
                        ListaInstructores.Add(cliente);
                    }
                }
                else {
                    ListaInstructores.Clear();
                    var filteredList = instructores.Where(c => c.Nombre.ToLower().Contains(query.ToLower()) || c.ApellidoPaterno.ToLower().Contains(query.ToLower()) || c.ApellidoMaterno.ToLower().Contains(query.ToLower())).ToList();
                    foreach (var cliente in filteredList) {
                        ListaInstructores.Add(cliente);
                    }
                }
            }
            else return;
        }

        public void OpenOverview() {
            OverviewInstructorWindow overview = new(selectedInstructor);
            overview.ShowDialog();
            RefreshGrid();
        }

        public void OpenWindow() {
            NewInstructorWindow window = new NewInstructorWindow();
            window.ShowDialog();
            RefreshGrid();
        }

        private async void RefreshGrid() {
            ListaInstructores.Clear();
            var instructors = await GetFromDb.GetInstructores();
            InitInfo.ListaInstructor = instructors;
            foreach (var item in instructors.OrderBy(i => i.Nombre)) {
                ListaInstructores.Add(item);
            }
        }

        public async void DeleteInstructor() {
            if (ShowPrettyMessages.QuestionYesNo("¿Desea borrar al instructor?", "Confirmación")) {
                await AdminUsuariosGeneral.Delete(SelectedInstructor);
                RefreshGrid();
            }
            else return;
        }

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

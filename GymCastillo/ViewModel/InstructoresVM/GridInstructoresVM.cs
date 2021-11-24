using GalaSoft.MvvmLight.Command;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes;
using GymCastillo.Model.Init;
using GymCastillo.Model.Interfaces;
using GymCastillo.View.InstructoresView;
using GymCastillo.ViewModel.Commands.InstructorsCommands;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GymCastillo.ViewModel.InstructoresVM {
    public class GridInstructoresVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);


        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Instructor> ListaInstructores { get; set; }

        public NewWindowInstructorCommand newWindow { get; set; }

        public OverviewInstructorCommand overviewInstructor { get; set; }

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
                //instructores = GetFromDb.GetInstructores().GetAwaiter().GetResult();
                ListaInstructores = new ObservableCollection<Instructor>();
                foreach (var item in instructores.OrderBy(i => i.Nombre)) {
                    ListaInstructores.Add(item);
                }

                Log.Debug("Inicializada la pantalla de instructores con éxito.");
            }
            catch (Exception e) {

                MessageBox.Show(e.Message);
            }


        }
        private void FilterList(string query) {
            if (instructores != null) {
                if (query == "") {
                    ListaInstructores.Clear();
                    foreach (var cliente in instructores.OrderBy(i => i.Nombre)) {
                        ListaInstructores.Add(cliente);
                    }
                }
                else {
                    ListaInstructores.Clear();
                    var filteredList = instructores.Where(c => c.Nombre.ToLower().Contains(query.ToLower())).ToList();
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

        private void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

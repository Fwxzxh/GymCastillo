using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using GymCastillo.Model.Admin;
using GymCastillo.Model.Database;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using GymCastillo.View.PersonalScreenView.InstructoresView;
using GymCastillo.ViewModel.PersonalScreensVM.Commands.InstructorsCommands;
using log4net;

namespace GymCastillo.ViewModel.PersonalScreensVM.InstructoresVM {
    public class GridInstructoresVM : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);


        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Instructor> ListaInstructores { get; set; }

        public NewWindowInstructorCommand newWindow { get; set; }

        public OverviewInstructorCommand overviewInstructor { get; set; }

        public DeleteInstructorCommand delete { get; set; }

        private ObservableCollection<Instructor> instructores { get; set; }

        private string query = "";

        public string Query {
            get { return query; }
            set
            {
                query = value;
                OnPropertyChanged(nameof(Query));
                FilterData(value);
            }
        }

        private static void FilterData(string value) {
            if (value != null) {
                CollectionViewSource.GetDefaultView(InitInfo.ObCoInstructor).Filter = item => (item as Instructor).Nombre.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
            }
            else CollectionViewSource.GetDefaultView(InitInfo.ObCoInstructor);
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
                delete = new(this);
                Log.Debug("Inicializada la pantalla de instructores con éxito.");
            }
            catch (Exception e) {
                Log.Error(e.Message);
                //MessageBox.Show(e.Message);
            }

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
            //ListaInstructores.Clear();
            InitInfo.ObCoInstructor.Clear();
            var instructors = await GetFromDb.GetInstructores();
            foreach (var item in instructors.OrderBy(i => i.Nombre)) {
                InitInfo.ObCoInstructor.Add(item);
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

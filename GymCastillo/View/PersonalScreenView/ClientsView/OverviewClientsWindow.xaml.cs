﻿using System.Text.RegularExpressions;
using System.Windows;
using GymCastillo.Model.DataTypes.Personal;
using GymCastillo.Model.Interfaces;
using GymCastillo.ViewModel.PersonalScreensVM.ClientsVM;

namespace GymCastillo.View.PersonalScreenView.ClientsView {
    /// <summary>
    /// Interaction logic for OverviewClientsWindow.xaml
    /// </summary>
    public partial class OverviewClientsWindow : Window, IClosable {
        public OverviewClientsWindow(Cliente cliente) {
            InitializeComponent();
            this.DataContext = new OverviewClientsVM(cliente);
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

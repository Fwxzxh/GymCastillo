using GymCastillo.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GymCastillo.View.AdminScreensView.PersonalView {
    /// <summary>
    /// Interaction logic for NewPersonalWindow.xaml
    /// </summary>
    public partial class NewPersonalWindow : Window, IClosable {
        public NewPersonalWindow() {
            InitializeComponent();
        }
    }
}

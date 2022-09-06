using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GymCastillo.ViewModel.Helpers {
    internal class RolVisibility : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var rol = (int)value;
            if (rol != 1) return "Hidden";
            else return "Visible";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}

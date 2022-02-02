using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GymCastillo.ViewModel.Helpers {
    public class BoolToActivo : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var respuesta = (bool)value;
            if (respuesta) {
                return "Encendido";
            }
            else return "Apagado";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }

    }
}

using GymCastillo.Model.Init;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GymCastillo.ViewModel.Helpers {
    public class IdToEspacio : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var id = (int)value;
            foreach (var item in InitInfo.ObCoEspacios) {
                if (id == item.IdEspacio) {
                    return item.NombreEspacio;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}

using GymCastillo.Model.Init;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GymCastillo.ViewModel.Helpers {
    public class IdToClienteR : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var id = (int)value;
            foreach (var item in InitInfo.ObCoClientesRenta) {
                if (id == item.Id) {
                    return $"{item.Nombre} {item.ApellidoPaterno} {item.ApellidoMaterno}";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}

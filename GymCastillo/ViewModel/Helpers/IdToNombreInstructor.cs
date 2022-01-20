using GymCastillo.Model.Init;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GymCastillo.ViewModel.Helpers {
    public class IdToNombreInstructor : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            int id = (int)value;
            foreach (var item in InitInfo.ObCoInstructor) {
                if (item.Id == id) {
                    return $"{item.Nombre} {item.ApellidoPaterno}";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}

using GymCastillo.Model.Helpers;
using GymCastillo.Model.Init;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GymCastillo.ViewModel.Helpers {
    public class IdToTotalAlumnosClase : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var idClase = (int)value;
            var totalClientes = ListaAlumnosHelper.GetClientesDeClase(idClase).Where(x => x.Activo == true).Count();
            return totalClientes;
                
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}

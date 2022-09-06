using GymCastillo.Model.Init;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GymCastillo.ViewModel.Helpers {
    internal class IdToNombreCliente : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var idCliente = (int)value;
            var cliente = InitInfo.ObCoClientes.Where(x => x.Id == idCliente).First();
            return $"{cliente.Nombre} {cliente.ApellidoPaterno} {cliente.ApellidoMaterno}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}

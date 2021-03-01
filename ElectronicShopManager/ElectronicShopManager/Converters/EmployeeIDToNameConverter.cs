using ElectronicShopManager.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ElectronicShopManager.Converters
{
    class EmployeeIDToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataAccessService dataService = new DataAccessService();
            Dictionary<int, string> IdNames = dataService.GetEmployees();

            return IdNames[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

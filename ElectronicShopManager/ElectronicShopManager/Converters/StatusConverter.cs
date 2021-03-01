using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace ElectronicShopManager.Converters
{
    class StatusConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value != null)
            {
                switch ((byte)value)
                {
                    case 0:
                        return "Order Pending";
                    case 1:
                        return "Order Confirmed";
                    case 2:
                        return "Order In Progress";
                    case 3:
                        return "Order Completed";
                    default:
                        break;
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

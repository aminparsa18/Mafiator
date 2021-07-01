using System;
using System.Globalization;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
   public class HubStatusConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool connected)
                return connected ? "Online" : "Offline";
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

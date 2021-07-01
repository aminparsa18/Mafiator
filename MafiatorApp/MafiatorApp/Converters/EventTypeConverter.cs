using System;
using System.Globalization;
using MafiatorApp.Enums;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
   public class EventTypeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GameEventType type)
            {
                if (type == GameEventType.Killed)
                    return "#CC000000";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

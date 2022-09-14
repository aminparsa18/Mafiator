using Mafiator.Common.Data.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
    public class EventTypeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is GameEventType.Killed ? "#CC000000" : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
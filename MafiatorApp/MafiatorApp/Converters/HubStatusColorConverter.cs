using System;
using System.Globalization;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
   public class HubStatusColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool connected)
                return connected ?  new SolidColorBrush(Color.Green) : new SolidColorBrush(Color.Red);
            return new SolidColorBrush(Color.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

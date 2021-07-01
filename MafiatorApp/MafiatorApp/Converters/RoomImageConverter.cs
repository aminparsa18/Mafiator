using System;
using System.Globalization;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
   public class RoomImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Constants.VaultUrl+"img/room/" + value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using MafiatorApp.Enums;
using MafiatorApp.Helpers;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
   public class RoleNameConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "Fetching Role...";
            return EnumHelper<GameRole>.GetDescriptionValue((GameRole)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Globalization;
using MafiatorApp.Enums;
using MafiatorApp.Helpers;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
   public class RoleImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            var address = "https://mftor.blob.core.windows.net/avatars/" +
                          EnumHelper<GameRole>.GetDescriptionValue((GameRole) value).ToLower() + ".png";
            return address;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

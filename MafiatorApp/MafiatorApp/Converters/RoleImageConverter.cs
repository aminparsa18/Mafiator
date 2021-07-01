using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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
            var address = "http://vault.mafiator.com/img/roles/" +
                          EnumHelper<GameRole>.GetDescriptionValue((GameRole) value) + ".png";
            return address;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

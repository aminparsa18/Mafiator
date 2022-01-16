using MafiatorApp.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
    public class PlayerStatusTintConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not PlayerStatus status)
                return "";
            return status switch
            {
                PlayerStatus.Playing => "#00000000",
                PlayerStatus.Killed => "#CC000000",
                _ => ""
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
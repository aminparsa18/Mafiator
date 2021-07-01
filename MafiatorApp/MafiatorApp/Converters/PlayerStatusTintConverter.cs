using System;
using System.Globalization;
using MafiatorApp.Enums;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
    public class PlayerStatusTintConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlayerStatus status)
            {
                if (status == PlayerStatus.Playing)
                    return "#00000000";
                if (status == PlayerStatus.Killed)
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
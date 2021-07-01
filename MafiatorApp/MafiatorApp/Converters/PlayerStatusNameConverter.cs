using System;
using System.Globalization;
using MafiatorApp.Enums;
using MafiatorApp.Helpers;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
    public class PlayerStatusNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlayerStatus status)
                return EnumHelper<PlayerStatus>.GetDescriptionValue(status);
            if(value is GameEventType eventType)
                return EnumHelper<GameEventType>.GetDescriptionValue(eventType);
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
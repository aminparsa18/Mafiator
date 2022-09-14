using Mafiator.Common.Data.Enums;
using MafiatorApp.Helpers;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
    public class PlayerStatusNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                PlayerStatus status => EnumHelper<PlayerStatus>.GetDescriptionValue(status),
                GameEventType eventType => EnumHelper<GameEventType>.GetDescriptionValue(eventType),
                _ => "Unknown"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
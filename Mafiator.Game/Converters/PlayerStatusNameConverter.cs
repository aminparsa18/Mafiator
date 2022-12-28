using Mafiator.Common.Data.Enums;
using Mafiator.Game.Helpers;
using System.Globalization;

namespace Mafiator.Game.Converters;

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
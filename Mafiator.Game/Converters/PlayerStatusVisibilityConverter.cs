using Mafiator.Common.Data.Enums;
using System.Globalization;

namespace Mafiator.Game.Converters;

public class PlayerStatusVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is PlayerStatus status)
            return status != PlayerStatus.Playing;
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
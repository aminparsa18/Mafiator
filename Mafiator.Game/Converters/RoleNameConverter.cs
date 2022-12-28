using Mafiator.Common.Data.Enums;
using Mafiator.Game.Helpers;
using System.Globalization;

namespace Mafiator.Game.Converters;

public class RoleNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? "Fetching Role..." : EnumHelper<GameRole>.GetDescriptionValue((GameRole)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
using Mafiator.Common.Data.Enums;
using Mafiator.Game.Helpers;
using System.Globalization;

namespace Mafiator.Game.Converters;

public class RoleImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return "";
        var address = string.Join("", "https://mftor.blob.core.windows.net/avatars/",
                      EnumHelper<GameRole>.GetDescriptionValue((GameRole)value).ToLower(), ".png");
        return address;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
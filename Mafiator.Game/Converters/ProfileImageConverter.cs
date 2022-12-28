using System.Globalization;

namespace Mafiator.Game.Converters;

public class ProfileImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Join("", "https://mftor.blob.core.windows.net/avatars/", value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
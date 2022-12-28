using System.Globalization;

namespace Mafiator.Game.Converters;

public class CountryCodelmageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Join("", "https://flagcdn.com/24x18/", value.ToString().ToLower(), ".png");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
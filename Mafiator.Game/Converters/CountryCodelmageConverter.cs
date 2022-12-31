using System.Globalization;

namespace Mafiator.Game.Converters;

public class CountryCodelmageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        string.Join("", "https://flagcdn.com/48x36/", value?.ToString().ToLower(), ".png");

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
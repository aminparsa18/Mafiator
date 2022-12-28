using System.Globalization;

namespace Mafiator.Game.Converters;

public class FirstValidationErrorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is ICollection<string> { Count: > 0 } errors ? errors.ElementAt(0) : null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
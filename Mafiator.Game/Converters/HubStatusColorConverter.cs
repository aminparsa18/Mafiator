using System.Globalization;

namespace Mafiator.Game.Converters;

public class HubStatusColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool connected)
            return connected ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        return new SolidColorBrush(Colors.White);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
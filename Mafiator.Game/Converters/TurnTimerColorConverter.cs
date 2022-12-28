using System.Globalization;

namespace Mafiator.Game.Converters;

public class TurnTimerColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string timer)
            return Colors.White;
        var span = TimeSpan.ParseExact(timer, @"mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None);
        return span.Seconds switch
        {
            <= 15 and > 10 => Colors.Yellow,
            <= 10 and > 5 => Colors.DarkOrange,
            <= 5 and > 0 => Colors.Red,
            _ => Colors.White
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
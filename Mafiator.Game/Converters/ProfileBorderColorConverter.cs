using System.Globalization;

namespace Mafiator.Game.Converters;

public class ProfileBorderColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Application.Current.RequestedTheme == AppTheme.Dark ? "#e0e1dd" : "#e0e1dc";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
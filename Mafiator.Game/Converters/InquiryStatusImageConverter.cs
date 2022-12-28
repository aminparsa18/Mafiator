using System.Globalization;

namespace Mafiator.Game.Converters;

public class InquiryStatusImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool inquiry)
            return inquiry ? "like.png" : "disslike.png";
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
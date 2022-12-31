using Mafiator.Game.Resources.Texts;
using System.Globalization;

namespace Mafiator.Game.Converters;

public class InquiryStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool inquiry)
            return inquiry ? LocalizationResourceManager.Instance["Positive"] : LocalizationResourceManager.Instance["Negative"];
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
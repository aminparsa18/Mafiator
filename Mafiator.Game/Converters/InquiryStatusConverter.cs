using Mafiator.Game.Resources.Texts;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Mafiator.Game.Converters;

public class InquiryStatusConverter : IValueConverter
{
    private readonly IStringLocalizer<AppResources> _localizer;

    public InquiryStatusConverter(IStringLocalizer<AppResources> localizer)
    {
        _localizer = localizer;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool inquiry)
            return inquiry ? _localizer["Positive"] : _localizer["Negative"];
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
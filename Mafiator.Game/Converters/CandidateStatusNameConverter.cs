using Mafiator.Game.Enums;
using Mafiator.Game.Helpers;
using System.Globalization;

namespace Mafiator.Game.Converters;

public class CandidateStatusNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is CandidateStatus status)
            return EnumHelper<CandidateStatus>.GetDescriptionValue(status);
        return "Unknown";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

using Mafiator.Game.Enums;
using System.Globalization;

namespace Mafiator.Game.Converters;

public class CandidateStatusVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is CandidateStatus status)
            return status != CandidateStatus.None;
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
using System;
using System.Globalization;
using MafiatorApp.Enums;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
    public class CandidateStatusVisibilityConverter:IValueConverter
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
}

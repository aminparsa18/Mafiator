using MafiatorApp.Enums;
using MafiatorApp.Helpers;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
    public class CandidateStatusNameConverter:IValueConverter
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
}

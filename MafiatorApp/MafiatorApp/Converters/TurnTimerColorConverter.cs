using System;
using System.Globalization;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
    public class TurnTimerColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string timer)) return Color.White;
            var span = TimeSpan.ParseExact(timer, @"mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None);
            if (span.Seconds <= 15 && span.Seconds > 10)
                return Color.Yellow;
            if (span.Seconds <= 10 && span.Seconds > 5)
                return Color.DarkOrange;
            if (span.Seconds <= 5 && span.Seconds > 0)
                return Color.Red;
            return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

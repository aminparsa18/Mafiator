using System;
using System.Globalization;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
    public class TurnTimerColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string timer) 
                return Color.White;
            var span = TimeSpan.ParseExact(timer, @"mm\:ss", CultureInfo.InvariantCulture, TimeSpanStyles.None);
            return span.Seconds switch
            {
                <= 15 and > 10 => Color.Yellow,
                <= 10 and > 5 => Color.DarkOrange,
                <= 5 and > 0 => Color.Red,
                _ => Color.White
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

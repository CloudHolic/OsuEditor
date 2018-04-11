using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace OsuEditor.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class MillisecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value != null, nameof(value) + " != null");
            var origin = (int) Math.Round(System.Convert.ToDouble(value));
            var isMinus = false;
            if (origin < 0)
            {
                origin *= -1;
                isMinus = true;
            }

            var min = origin / 60000;
            var sec = (origin - (min * 60000)) / 1000;
            var millisec = origin - (min * 60000) - (sec * 1000);
            return (isMinus ? "-" : string.Empty) + $"{min:00}:{sec:00}.{millisec:000}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

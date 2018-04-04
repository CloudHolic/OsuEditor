using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace OsuEditor.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class SpeedRateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value != null, nameof(value) + " != null");
            var origin = (double) System.Convert.ToInt32(value) / 100;
            return $"x{origin:0.00}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

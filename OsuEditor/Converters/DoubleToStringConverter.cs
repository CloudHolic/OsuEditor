using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using OsuEditor.Util;

namespace OsuEditor.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value != null, nameof(value) + " != null");
            var origin = (double) value;
            return Math.Abs(origin % 1) < 0.01 ? value.ToString().Split('.')[0] : value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bounds = parameter == null ? new[] { double.MinValue, double.MaxValue } : parameter as double[];
            Debug.Assert(bounds != null, nameof(bounds) + " != null");
            double.TryParse((string)value, out var converted);
            return MathExt.Clamp(converted, bounds[0], bounds[1]);
        }
    }
}

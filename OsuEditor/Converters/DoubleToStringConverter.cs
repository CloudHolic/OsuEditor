using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace OsuEditor.Converters
{
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
            double.TryParse((string)value, out var converted);
            return converted;
        }
    }
}

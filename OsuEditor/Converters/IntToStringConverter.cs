using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using OsuEditor.Util;

namespace OsuEditor.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value != null, nameof(value) + " != null");
            return System.Convert.ToInt32(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bounds = parameter == null ? new[] { int.MinValue, int.MaxValue } : parameter as int[];
            Debug.Assert(bounds != null, nameof(bounds) + " != null");
            int.TryParse((string)value, out var converted);
            return MathExt.Clamp(converted, bounds[0], bounds[1]);
        }
    }
}

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using OsuEditor.Models;

namespace OsuEditor.Converters
{
    [ValueConversion(typeof(PlayMode), typeof(Visibility))]
    public class PlayModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value != null, nameof(value) + " != null");
            Debug.Assert(parameter != null, nameof(parameter) + " != null");

            var curMode = value is int ? System.Convert.ToInt32(value) : (int) (PlayMode) value;
            var originParameter = System.Convert.ToInt32(parameter);
            var visibleModes = originParameter == 4 ? new[] {0, 2} : new[] {originParameter};

            return Array.Exists(visibleModes, x => x == curMode) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OsuEditor.Converters
{
    [ValueConversion(typeof(bool), typeof(Brush))]
    public class BooleanToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value != null, nameof(value) + " != null");
            var backgrounds = parameter as Brush[];
            Debug.Assert(backgrounds != null, nameof(backgrounds) + " != null");

            return System.Convert.ToBoolean(value) ? backgrounds[0] : backgrounds[1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

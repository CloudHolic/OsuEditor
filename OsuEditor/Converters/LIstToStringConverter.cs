using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace OsuEditor.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tagList = (List<string>) value;
            var tagArray = tagList?.ToArray();
            return tagArray == null ? null : string.Join(" ", tagArray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tagArray = ((string) value)?.Split(' ');
            return tagArray?.ToList();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;
using OsuParser.Structures;

namespace OsuEditor.Converters
{
    [ValueConversion(typeof(Metadata), typeof(string))]
    public class MainWindowTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var title = "Osu! Editor";
            if (value is Metadata origin)
            {
                if (origin.Artist != null && origin.Title != null)
                    title += $" | {origin.Artist} - {origin.Title}";
                if (origin.Creator != null)
                    title += $" ({origin.Creator})";
                if (origin.Version != null)
                    title += $" [{origin.Version}]";
            }

            return title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

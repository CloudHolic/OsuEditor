using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using OsuEditor.CustomExceptions;
using OsuEditor.Events;

namespace OsuEditor.Converters
{
    public class BeatSnapToSliderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value != null, nameof(value) + " != null");

            var beatSnapList = new[] { 1, 2, 3, 4, 6, 8, 12, 16 }.ToList();
            var sliderValueList = new[] { 0, 2, 3, 4, 6, 8, 12, 16 };

            var index = beatSnapList.FindIndex(x => x == (int)value);
            if (index > -1 && index < 10)
                return sliderValueList[index];

            throw new InvalidValueException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value != null, nameof(value) + " != null");

            var sliderValueList = new[] { 0, 2, 3, 4, 6, 8, 12, 16 }.ToList();
            var beatSnapList = new[] { 1, 2, 3, 4, 6, 8, 12, 16 };

            var index = sliderValueList.FindIndex(x => x == System.Convert.ToInt32(value));
            if (index > -1 && index < 10)
            {
                var snap = beatSnapList[index];
                EventBus.Instance.Publish(new BeatSnapEvent {Snap = snap});
                return snap;
            }

            throw new InvalidValueException();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using OsuEditor.CustomExceptions;
using OsuEditor.Events;

namespace OsuEditor.Contents
{
    public partial class ComposeHeaderView
    {
        public ComposeHeaderView()
        {
            InitializeComponent();
        }

        private void BeatSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BeatLabel == null)
                return;

            var beatSnap = SliderToBeatSnap(e.NewValue);
            BeatLabel.Content = $"1/{beatSnap}";

            EventBus.Instance.Publish(new BeatSnapEvent {Snap = beatSnap});
        }

        private int SliderToBeatSnap(double sliderValue)
        {
            var sliderValueList = new[] {0, 2, 3, 4, 6, 8, 10, 12, 14, 16}.ToList();
            var beatSnapList = new[] {1, 2, 3, 4, 6, 8, 12, 16, 24, 32};

            var index = sliderValueList.FindIndex(x => x == (int) sliderValue);
            if (index > -1 && index < 10)
                return beatSnapList[index];

            throw new InvalidValueException();
        }
    }
}

using System.Linq;
using System.Windows;
using OsuEditor.CustomExceptions;
using OsuEditor.Events;
using OsuEditor.ViewModels;

namespace OsuEditor.Contents
{
    public partial class ComposeHeaderView
    {
        public ComposeHeaderView(int snap)
        {
            InitializeComponent();
            DataContext = new ComposeHeaderViewModel(snap);
        }

        private void BeatSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BeatLabel == null)
                return;

            var beatSnap = SliderToBeatSnap(e.NewValue);

            EventBus.Instance.Publish(new BeatSnapEvent {Snap = beatSnap});
        }

        private static int SliderToBeatSnap(double sliderValue)
        {
            var sliderValueList = new[] { 0, 2, 3, 4, 6, 8, 12, 16 }.ToList();
            var beatSnapList = new[] { 1, 2, 3, 4, 6, 8, 12, 16 };

            var index = sliderValueList.FindIndex(x => x == (int)sliderValue);
            if (index > -1 && index < 10)
                return beatSnapList[index];

            throw new InvalidValueException();
        }
    }
}

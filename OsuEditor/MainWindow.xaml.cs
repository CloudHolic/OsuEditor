using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using OsuEditor.CustomExceptions;
using OsuEditor.Events;
using OsuEditor.ViewModels;

namespace OsuEditor
{
    public partial class MainWindow : IEvent<CurPositionEvent>, IEvent<TimingChangedEvent>
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWIndowViewModel();
            EventBus.Instance.RegisterHandler(this);
        }

        private void IncreaseZoom_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWIndowViewModel)DataContext).Zoom = HeaderTimeline.Zoom = Math.Min(30, HeaderTimeline.Zoom + 0.1);
        }

        private void DecreaseZoom_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWIndowViewModel)DataContext).Zoom = HeaderTimeline.Zoom = Math.Max(0, HeaderTimeline.Zoom - 0.1);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            HeaderTimeline.MaxWidth = HeaderTimeline.Width = HeaderGrid.ColumnDefinitions[0].ActualWidth - 40;
        }

        private void BeatSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BeatLabel == null)
                return;

            var beatSnap = SliderToBeatSnap(e.NewValue);

            HeaderTimeline.BeatSnap = beatSnap;

            EventBus.Instance.Publish(new BeatSnapEvent { Snap = beatSnap });
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

        #region Event Handlers
        public void HandleEvent(CurPositionEvent e)
        {
            HeaderTimeline.CurrentValue = e.CurPosition;
        }

        public void HandleEvent(TimingChangedEvent e)
        {
            HeaderTimeline.Timings = e.NewTiming;
        }
        #endregion
    }
}

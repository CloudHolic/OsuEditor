using System;
using System.Windows;
using OsuEditor.Events;
using OsuEditor.ViewModels;

namespace OsuEditor
{
    public partial class MainWindow : IEvent<BeatSnapEvent>, IEvent<CurPositionEvent>, IEvent<TimingChangedEvent>
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

        #region Event Handlers
        public void HandleEvent(BeatSnapEvent e)
        {
            HeaderTimeline.BeatSnap = ((MainWIndowViewModel) DataContext).Snap = e.Snap;
        }

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

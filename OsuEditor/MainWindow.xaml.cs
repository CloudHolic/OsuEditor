using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OsuEditor.Events;
using OsuEditor.ViewModels;

namespace OsuEditor
{
    public partial class MainWindow : IEvent<CurPositionEvent>, IEvent<TimingChangedEvent>, IEvent<BeatSnapEvent>
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWIndowViewModel();
            EventBus.Instance.RegisterHandler(this);
        }

        private void ZoomIn_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWIndowViewModel) DataContext).CurrentMap.Edit.TimelineZoom =
                HeaderTimeline.Zoom = Math.Max(0, HeaderTimeline.Zoom - 0.1);
        }

        private void ZoomOut_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWIndowViewModel) DataContext).CurrentMap.Edit.TimelineZoom =
                HeaderTimeline.Zoom = Math.Min(30, HeaderTimeline.Zoom + 0.1);
        }
        
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            HeaderTimeline.MaxWidth = HeaderTimeline.Width = HeaderGrid.ColumnDefinitions[0].ActualWidth - 40;
        }

        private void DoubleTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != ".")
            {
                foreach (var c in e.Text)
                {
                    if (!char.IsDigit(c))
                    {
                        e.Handled = true;
                        break;
                    }
                }
            }
        }
        
        private void IntTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (var c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }
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

        public void HandleEvent(BeatSnapEvent e)
        {
            HeaderTimeline.BeatSnap = e.Snap;
        }
        #endregion
    }
}

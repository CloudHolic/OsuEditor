using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using OsuEditor.Events;
using OsuEditor.Models;
using OsuEditor.ViewModels;

namespace OsuEditor
{
    public partial class MainWindow : IEvent<BeatSnapEvent>, IEvent<CurPositionEvent>, IEvent<TimingChangedEvent>, IEvent<CurrentTimingChangedEvent>
    {
        private int _prevOffset;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            EventBus.Instance.RegisterHandler(this);
        }

        private void ZoomIn_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel) DataContext).CurrentMap.Edit.TimelineZoom =
                HeaderTimeline.Zoom = Math.Max(0, HeaderTimeline.Zoom - 0.1);
        }

        private void ZoomOut_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel) DataContext).CurrentMap.Edit.TimelineZoom =
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
        
        private void OffsetTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            _prevOffset = Convert.ToInt32(OffsetTextBox.Text);
        }

        private void OffsetTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var viewModel = (MainWindowViewModel) DataContext;
            var timings = viewModel.TimingMarks;
            var current = viewModel.CurrentTiming;
            foreach (var timing in timings)
            {
                if (current == timing)
                    continue;

                if (timing.Offset == Convert.ToInt32(OffsetTextBox.Text))
                {
                    viewModel.OffsetErrorOccurred = true;
                    if(viewModel.ErrorTimer.IsEnabled)
                        viewModel.ErrorTimer.Stop();
                    viewModel.ErrorTimer.Start();
                    OffsetTextBox.Text = _prevOffset.ToString();
                }
            }
        }

        private void BpmTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            EventBus.Instance.Publish(new CurrentTimingChangedEvent
            {
                Bpm = Convert.ToDouble(BpmTextBox.Text),
                Offset = Convert.ToInt32(OffsetTextBox.Text)
            });
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

        public void HandleEvent(CurrentTimingChangedEvent e)
        {
            var marks = ((MainWindowViewModel) DataContext).TimingMarks;

            if (marks.Count == 0)
                NewBaseCheckBox.IsHitTestVisible = true;
            if (marks.Count == 1)
            {
                NewBaseCheckBox.IsChecked = true;
                NewBaseCheckBox.IsHitTestVisible = false;
            }
            else
            {
                //  Find previous TimingMark
                TimingMark prevMark = null;
                foreach (var timing in marks)
                    if (e.Offset > timing.Offset)
                        prevMark = timing;

                if (prevMark == null)
                    prevMark = marks.First();

                if (Math.Abs(prevMark.Bpm - e.Bpm) < 0.001)
                    NewBaseCheckBox.IsHitTestVisible = true;
                else
                {
                    NewBaseCheckBox.IsChecked = true;
                    NewBaseCheckBox.IsHitTestVisible = false;
                }
            }
        }
        #endregion
    }
}

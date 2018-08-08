using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using OsuEditor.Events;
using OsuEditor.Models.Dialogs;
using OsuEditor.Models.Timings;
using OsuEditor.Util;
using OsuEditor.ViewModels;

namespace OsuEditor
{
    public partial class MainWindow : IEvent<BeatSnapEvent>, IEvent<CurPositionEvent>, IEvent<TimingChangedEvent>, IEvent<CurrentTimingChangedEvent>, IEvent<ChangeCurrentMapEvent>
    {
        private int _prevOffset;
        private readonly OpenSettings _settings;

        public MainWindow(OpenSettings settings)
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            EventBus.Instance.RegisterHandler(this);

            _settings = settings;
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

            ((MainWindowViewModel) DataContext).InitSettings(_settings);
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
                    if (viewModel.ErrorTimer.IsEnabled)
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

        private void DiffListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DiffListBox.SelectedIndex == -1)
                return;

            foreach (var diff in ((MainWindowViewModel)DataContext).Diffs)
                diff.Activated = diff == ((MainWindowViewModel)DataContext).CurrentDiff;

            EventBus.Instance.Publish(new ChangeCurrentMapEvent
            {
                OsuFileName = ((MainWindowViewModel)DataContext).CurrentDiff.FileName
            });
        }
        
        private void MainMusicBar_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EventBus.Instance.Publish(new CurPositionEvent
            {
                CurPosition = ((MainWindowViewModel) DataContext).CurrentPosition
            });
        }
        
        private void TimingListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TimingListView.SelectedIndex == -1)
                return;

            ((MainWindowViewModel) DataContext).CurrentPosition = ((MainWindowViewModel) DataContext).CurrentTiming.Offset;
            EventBus.Instance.Publish(new CurPositionEvent
            {
                CurPosition = ((MainWindowViewModel)DataContext).CurrentPosition
            });
        }

        private void AddLine(double offset)
        {
            var cor = MathExt.Clamp(offset / ((MainWindowViewModel) DataContext).SongLength * LineCanvas.Width, 0, LineCanvas.ActualWidth);

            var line = new Line
            {
                X1 = cor,
                X2 = cor,
                Y1 = 0,
                Y2 = 30,
                StrokeThickness = 1,
                Stroke = Brushes.Violet
            };
            line.MouseUp += Line_OnMouseUp;

            LineCanvas.Children.Add(line);
        }
        
        private void Line_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            var line = (Line) e.Source;
            var offset = line.X1 / LineCanvas.Width * ((MainWindowViewModel) DataContext).SongLength;

            foreach (var timing in TimingListView.Items)
            {
                var item = (TimingMark) timing;
                if (Math.Abs(item.Offset - offset) < 1)
                {
                    ((MainWindowViewModel) DataContext).CurrentTiming = item;
                    break;
                }
            }

            var doubleClickEvent = new MouseButtonEventArgs(Mouse.PrimaryDevice, new TimeSpan(DateTime.Now.Ticks).Milliseconds, MouseButton.Left)
            {
                RoutedEvent = MouseDoubleClickEvent
            };

            TimingListView.RaiseEvent(doubleClickEvent);
        }

        #region Event Handlers
        public void HandleEvent(CurPositionEvent e)
        {
            HeaderTimeline.CurrentValue = e.CurPosition;

            var dataContext = (MainWindowViewModel)DataContext;
            TimingMark prevMark = null;

            foreach(var mark in dataContext.TimingMarks)
            {
                if (mark.Offset > e.CurPosition)
                {
                    dataContext.CurrentTiming = prevMark ?? dataContext.TimingMarks[0];
                    break;
                }

                prevMark = mark;
            }
        }

        public void HandleEvent(TimingChangedEvent e)
        {
            HeaderTimeline.Timings = e.NewTiming;
            MainMusicBar.Timings = e.NewTiming;

            HeaderTimeline.TotalLength = e.Length;
        }

        public void HandleEvent(BeatSnapEvent e)
        {
            HeaderTimeline.BeatSnap = e.Snap;
        }

        public void HandleEvent(CurrentTimingChangedEvent e)
        {
            var marks = ((MainWindowViewModel)DataContext).TimingMarks;

            LineCanvas.Children.Clear();
            foreach (var item in marks)
                AddLine(item.Offset);

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

        public void HandleEvent(ChangeCurrentMapEvent e)
        {
            var marks = ((MainWindowViewModel)DataContext).TimingMarks;

            LineCanvas.Children.Clear();
            foreach (var item in marks)
                AddLine(item.Offset);
        }
        #endregion
    }
}

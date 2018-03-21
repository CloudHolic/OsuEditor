using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using OsuEditor.CustomExceptions;
using OsuEditor.Models;

namespace OsuEditor.Controls
{
    [TemplatePart(Name = "trackLine", Type = typeof(Line))]
    public class TimelineControl : Control
    {
        #region MouseMoveRoutedEvent
        public new static readonly RoutedEvent MouseMoveEvent = EventManager.RegisterRoutedEvent(
            "MouseMove", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TimelineControl));

        public new event RoutedEventHandler MouseMove
        {
            add => AddHandler(MouseMoveEvent, value);
            remove => RemoveHandler(MouseMoveEvent, value);
        }
        #endregion

        #region DependencyProperty MarkLengthProperty
        public static readonly DependencyProperty MarkLengthProperty =
            DependencyProperty.Register("MarkLengthProperty", typeof(int), typeof(TimelineControl),
                new UIPropertyMetadata(20));

        public int MarkLength
        {
            get => (int)GetValue(MarkLengthProperty);
            set => SetValue(MarkLengthProperty, value);
        }
        #endregion
        #region DependencyProperty MiddleMarkLengthProperty
        public static readonly DependencyProperty MiddleMarkLengthProperty =
            DependencyProperty.Register("MiddleMarkLengthProperty", typeof(int), typeof(TimelineControl),
                new UIPropertyMetadata(10));

        public int MiddleMarkLength
        {
            get => (int)GetValue(MiddleMarkLengthProperty);
            set => SetValue(MiddleMarkLengthProperty, value);
        }
        #endregion
        #region DependencyProperty LittleMarkLengthProperty
        public static readonly DependencyProperty LittleMarkLengthProperty =
            DependencyProperty.Register("LittleMarkLengthProperty", typeof(int), typeof(TimelineControl),
                new UIPropertyMetadata(5));

        public int LittleMarkLength
        {
            get => (int)GetValue(LittleMarkLengthProperty);
            set => SetValue(LittleMarkLengthProperty, value);
        }
        #endregion

        #region DependencyProperty TimingsProperty
        public static readonly DependencyProperty TimingsProperty =
            DependencyProperty.Register("TimingsProperty", typeof(Timeline), typeof(TimelineControl),
                new FrameworkPropertyMetadata(new Timeline(), FrameworkPropertyMetadataOptions.AffectsRender));

        public Timeline Timings
        {
            get => (Timeline) GetValue(TimingsProperty);
            set => SetValue(TimingsProperty, value);
        }
        #endregion

        #region DependencyProperty CurrentValueProperty
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValueProperty", typeof(double), typeof(TimelineControl),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double CurrentValue
        {
            get => (double)GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
        }
        #endregion
        #region DependencyProperty TotalLengthProperty
        public static readonly DependencyProperty TotalLengthProperty =
            DependencyProperty.Register("TotalLengthProperty", typeof(double), typeof(TimelineControl),
                new UIPropertyMetadata(10000.0));

        public double TotalLength
        {
            get => (double)GetValue(TotalLengthProperty);
            set => SetValue(TotalLengthProperty, value);
        }
        #endregion

        #region DependencyProperty ZoomProperty
        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("ZoomProperty", typeof(double), typeof(TimelineControl),
                new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }
        #endregion

        //private Point _mousePosition;
        //private Pen _mouseTrackPen = new Pen(new SolidColorBrush(Colors.Black), 1);
        private Line _mouseVerticalTrackLine;
        private Line _mouseHorizontalTrackLine;

        static TimelineControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineControl), new FrameworkPropertyMetadata(typeof(TimelineControl)));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if(CurrentValue < 0 || CurrentValue > TotalLength)
                throw new InvalidValueException();

            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 5), new Point(ActualWidth / 2, 0), new Point(ActualWidth / 2, ActualHeight));

            var transform = CurrentValue - ActualWidth / 2;

            var beats = new List<double>();
            var startIndex = (int)Math.Ceiling(transform / Timings.BeatLength[0]);
            var endIndex = (int)Math.Truncate((transform + ActualWidth) / Timings.BeatLength[0]);
            for (var i = startIndex; i <= endIndex; i++)
                beats.Add(i * Timings.BeatLength[0] - transform);
            
            foreach (var beat in beats)
            {
                if (Math.Abs((beat + transform) / Timings.BeatLength[0] % Timings.BeatsPerMeasure[0]) < 0.001)
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 2),
                        new Point(beat, ActualHeight - MarkLength), new Point(beat, ActualHeight));
                else
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 1),
                        new Point(beat, ActualHeight - MiddleMarkLength), new Point(beat, ActualHeight));

                //  Draw previous snaps if it's the first beat
                if (beats.IndexOf(beat) == 0)
                {
                    for (var j = 1; j < Timings.BeatSnap; j++)
                    {
                        var snapCor = beat - j * Timings.BeatLength[0] / Timings.BeatSnap;
                        if (snapCor < 0)
                            break;

                        drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 1),
                            new Point(snapCor, ActualHeight - LittleMarkLength), new Point(snapCor, ActualHeight));
                    }
                }

                for (var j = 1; j < Timings.BeatSnap; j++)
                {
                    var snapCor = beat + j * Timings.BeatLength[0] / Timings.BeatSnap;
                    if (snapCor > ActualWidth)
                        break;

                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 1),
                        new Point(snapCor, ActualHeight - LittleMarkLength), new Point(snapCor, ActualHeight));
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {

        }

        public void RaiseHorizontalRulerMoveEvent(MouseEventArgs e)
        {
            var mousePoint = e.GetPosition(this);
            _mouseHorizontalTrackLine.X1 = _mouseHorizontalTrackLine.X2 = mousePoint.X;
        }

        public void RaiseVerticalRulerMoveEvent(MouseEventArgs e)
        {
            var mousePoint = e.GetPosition(this);
            _mouseVerticalTrackLine.Y1 = _mouseVerticalTrackLine.Y2 = mousePoint.Y;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mouseVerticalTrackLine = GetTemplateChild("verticalTrackLine") as Line;
            _mouseHorizontalTrackLine = GetTemplateChild("horizontalTrackLine") as Line;
            _mouseVerticalTrackLine.Visibility = Visibility.Visible;
            if (_mouseHorizontalTrackLine != null)
                _mouseHorizontalTrackLine.Visibility = Visibility.Visible;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using OsuEditor.CustomExceptions;
using OsuEditor.Models;
using OsuEditor.Util;

namespace OsuEditor.Controls
{
    [TemplatePart(Name = "PART_Line", Type = typeof(Line))]
    public class Timeline : Control
    {
        #region MouseMoveRoutedEvent
        public new static readonly RoutedEvent MouseMoveEvent = EventManager.RegisterRoutedEvent(
            "MouseMove", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Timeline));

        public new event RoutedEventHandler MouseMove
        {
            add => AddHandler(MouseMoveEvent, value);
            remove => RemoveHandler(MouseMoveEvent, value);
        }
        #endregion

        #region DependencyProperty TimingsProperty
        public static readonly DependencyProperty TimingsProperty =
            DependencyProperty.Register("TimingsProperty", typeof(Timing), typeof(Timeline),
                new FrameworkPropertyMetadata(new Timing(), FrameworkPropertyMetadataOptions.AffectsRender));

        public Timing Timings
        {
            get => (Timing) GetValue(TimingsProperty);
            set => SetValue(TimingsProperty, value);
        }
        #endregion
        #region DependencyProperty BeatSnapProperty
        public static readonly DependencyProperty BeatSnapProperty = 
            DependencyProperty.Register("BeatSnapProperty", typeof(int), typeof(Timeline),
                new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.AffectsRender));

        public int BeatSnap
        {
            get => (int) GetValue(BeatSnapProperty);
            set => SetValue(BeatSnapProperty, value);
        }
        #endregion

        #region DependencyProperty CurrentValueProperty
        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register("CurrentValueProperty", typeof(double), typeof(Timeline),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public double CurrentValue
        {
            get => (double)GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
        }
        #endregion
        #region DependencyProperty TotalLengthProperty
        public static readonly DependencyProperty TotalLengthProperty =
            DependencyProperty.Register("TotalLengthProperty", typeof(double), typeof(Timeline),
                new UIPropertyMetadata(100000.0));

        public double TotalLength
        {
            get => (double)GetValue(TotalLengthProperty);
            set => SetValue(TotalLengthProperty, value);
        }
        #endregion

        #region DependencyProperty ZoomProperty
        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("ZoomProperty", typeof(double), typeof(Timeline),
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

        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if(CurrentValue < 0 || CurrentValue > TotalLength)
                throw new InvalidValueException();

            if (!MathExt.CompareInts(Timings.BeatLength.Count, Timings.BeatsPerMeasure.Count, Timings.Offset.Count))
                throw new InvalidValueException();
            
            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 5), new Point(ActualWidth / 2, 0), new Point(ActualWidth / 2, ActualHeight));

            var transform = (CurrentValue / Zoom) - (ActualWidth / 2);
            var timingPeriods = Timings.Offset.Count;
            
            var beatLists = new List<List<double>>();
            for (var i = 0; i < timingPeriods; i++)
            {
                var beats = new List<double>();
                var beatLength = Timings.BeatLength[i] / Zoom;
                var offset = Timings.Offset[i] / Zoom;

                var startIndex = (int) Math.Ceiling((transform - offset) / beatLength);
                var endIndex = (int) Math.Truncate((transform + ActualWidth - offset) / beatLength);
                for (var j = startIndex; j <= endIndex; j++)
                    beats.Add((j * beatLength) - transform + offset);

                beatLists.Add(beats);
            }

            for (var i = 0; i < beatLists.Count; i++)
            {
                var beats = beatLists[i];
                var curBeatLength = Timings.BeatLength[i] / Zoom;
                var offset = Timings.Offset[i] / Zoom;
                var nextOffset = i == beatLists.Count - 1 ? TotalLength + 1 : Timings.Offset[i + 1] / Zoom;

                foreach (var beat in beats)
                {
                    if (beat >= nextOffset - transform || (i > 0 && beat < offset - transform))
                        continue;

                    var beatNumber = Math.Abs(((beat + transform - offset) / curBeatLength) % Timings.BeatsPerMeasure[i]);
                    if (beatNumber < 0.001 || beatNumber > Timings.BeatsPerMeasure[i] - 0.001)
                        drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 2),
                            new Point(beat, ActualHeight - 50), new Point(beat, ActualHeight));
                    else
                        drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 1),
                            new Point(beat, ActualHeight - 30), new Point(beat, ActualHeight));

                    //  Draw previous snaps if it's the first beat
                    if (beats.IndexOf(beat) == 0)
                    {
                        for (var j = 1; j < BeatSnap; j++)
                        {
                            var snapCor = beat - ((j * curBeatLength) / BeatSnap);
                            if (snapCor < 0 || (i > 0 && snapCor < offset - transform))
                                break;

                            var height = CalcLineHeight(BeatSnap, j);
                            drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 1),
                                new Point(snapCor, ActualHeight - height), new Point(snapCor, ActualHeight));
                        }
                    }

                    for (var j = 1; j < BeatSnap; j++)
                    {
                        var snapCor = beat + ((j * curBeatLength) / BeatSnap);
                        if (snapCor > ActualWidth || snapCor >= nextOffset - transform)
                            break;

                        var height = CalcLineHeight(BeatSnap, j);
                        drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 1),
                            new Point(snapCor, ActualHeight - height), new Point(snapCor, ActualHeight));
                    }
                }
            }

            //TODO: Draw notes, preview points, SV points, break periods
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

        private static int CalcLineHeight(int snap, int cur)
        {
            var availableSnaps = new[] {2, 3, 4, 6, 8, 12, 16};
            if (Array.IndexOf(availableSnaps, snap) == -1)
                throw new InvalidValueException();

            if (snap % 3 == 0)
            {
                if (snap == 3 || cur % (snap / 3) == 0)
                    return 10;
                if (snap == 6 || cur % (snap / 6) == 0)
                    return 10;
                return 5;
            }

            if (snap == 2 || cur % (snap / 2) == 0)
                return 16;
            if (snap == 4 || cur % (snap / 4) == 0)
                return 8;
            if (snap == 8 || cur % (snap / 8) == 0)
                return 4;
            return 2;
        }
    }
}

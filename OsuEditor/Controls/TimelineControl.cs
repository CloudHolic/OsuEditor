using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OsuEditor.Controls
{
    #region Enumerations
    public enum EnumOrientation { Horizontal, Vertical }
    #endregion

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

        #region DependencyProperty OrientationProperty
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("DisplayMode", typeof(EnumOrientation), typeof(TimelineControl),
            new UIPropertyMetadata(EnumOrientation.Horizontal));

        public EnumOrientation Orientation
        {
            get => (EnumOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        #endregion
        #region DependencyProperty MajorIntervalProperty
        public static readonly DependencyProperty MajorIntervalProperty =
            DependencyProperty.Register("MajorIntervalProperty", typeof(int), typeof(TimelineControl),
            new UIPropertyMetadata(100));

        public int MajorInterval
        {
            get => (int)GetValue(MajorIntervalProperty);
            set => SetValue(MajorIntervalProperty, value);
        }
        #endregion
        #region DependencyProperty SubIntervalProperty
        public static readonly DependencyProperty SubIntervalProperty =
            DependencyProperty.Register("SubIntervalProperty", typeof(int), typeof(TimelineControl),
                new UIPropertyMetadata(4));

        public int SubInterval
        {
            get => (int)GetValue(SubIntervalProperty);
            set => SetValue(SubIntervalProperty, value);
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
        #region DependencyProperty StartValueProperty
        public static readonly DependencyProperty StartValueProperty =
            DependencyProperty.Register("StartValueProperty", typeof(double), typeof(TimelineControl),
            new UIPropertyMetadata(0.0));

        public double StartValue
        {
            get => (double)GetValue(StartValueProperty);
            set => SetValue(StartValueProperty, value);
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
            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            #region Horizontal Ruler
            if (Orientation == EnumOrientation.Horizontal)
            {
                for (var i = 0; i < ActualWidth / MajorInterval; i++)
                {
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 2), new Point(i * MajorInterval, ActualHeight - MarkLength), new Point(i * MajorInterval,ActualHeight));
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 1),
                        new Point(i * MajorInterval + MajorInterval / 2, ActualHeight - MiddleMarkLength),
                        new Point(i * MajorInterval + MajorInterval / 2, ActualHeight));
                    for (var j = 1; j < 10; j++)
                    {
                        if (j == 5)
                            continue;
                        
                        drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 1),
                        new Point(i * MajorInterval + MajorInterval * j / 10, ActualHeight - LittleMarkLength),
                        new Point(i * MajorInterval + MajorInterval * j / 10, ActualHeight));
                    }
                }
            }
            #endregion
            #region Vertical Ruler
            else
            {
                for (var i = 0; i < ActualHeight / MajorInterval; i++)
                {
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 1), new Point(ActualWidth - MarkLength, i * MajorInterval), new Point(ActualWidth, i * MajorInterval));
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 1),
                        new Point(ActualWidth - MiddleMarkLength, i * MajorInterval + MajorInterval / 2),
                        new Point(ActualWidth, i * MajorInterval + MajorInterval / 2));
                    for (var j = 1; j < 10; j++)
                    {
                        if (j==5)
                            continue;
                        
                        drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White), 1),
                        new Point(ActualWidth - LittleMarkLength, i * MajorInterval + MajorInterval*j / 10),
                        new Point(ActualWidth, i * MajorInterval + MajorInterval*j / 10));
                    }
                }
            }
            #endregion
            
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

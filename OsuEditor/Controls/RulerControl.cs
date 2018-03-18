using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OsuEditor.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RulerControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RulerControl;assembly=RulerControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    /// 
    #region Enumerations
    public enum EnumOrientation { Horizontal, Vertical }
    #endregion

    [TemplatePart(Name = "trackLine", Type = typeof(Line))]
    public class RulerControl : Control
    {
        #region MouseMoveRoutedEvent
        public new static readonly RoutedEvent MouseMoveEvent = EventManager.RegisterRoutedEvent(
            "MouseMove", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RulerControl));

        public new event RoutedEventHandler MouseMove
        {
            add => AddHandler(MouseMoveEvent, value);
            remove => RemoveHandler(MouseMoveEvent, value);
        }
        #endregion

        #region DepencyProperty OrientationProperty
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("DisplayMode", typeof(EnumOrientation), typeof(RulerControl),
            new UIPropertyMetadata(EnumOrientation.Horizontal));

        public EnumOrientation Orientation
        {
            get => (EnumOrientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        #endregion
        #region DepencyProperty MajorIntervalProperty
        public static readonly DependencyProperty MajorIntervalProperty =
            DependencyProperty.Register("MajorIntervalProperty", typeof(int), typeof(RulerControl),
            new UIPropertyMetadata(100));

        public int MajorInterval
        {
            get => (int)GetValue(MajorIntervalProperty);
            set => SetValue(MajorIntervalProperty, value);
        }
        #endregion
        #region DepencyProperty MarkLengthProperty
        public static readonly DependencyProperty MarkLengthProperty =
            DependencyProperty.Register("MarkLengthProperty", typeof(int), typeof(RulerControl),
            new UIPropertyMetadata(20));

        public int MarkLength
        {
            get => (int)GetValue(MarkLengthProperty);
            set => SetValue(MarkLengthProperty, value);
        }
        #endregion
        #region DepencyProperty MiddleMarkLengthProperty
        public static readonly DependencyProperty MiddleMarkLengthProperty =
            DependencyProperty.Register("MiddleMarkLengthProperty", typeof(int), typeof(RulerControl),
            new UIPropertyMetadata(10));

        public int MiddleMarkLength
        {
            get => (int)GetValue(MiddleMarkLengthProperty);
            set => SetValue(MiddleMarkLengthProperty, value);
        }
        #endregion
        #region DepencyProperty LittleMarkLengthProperty
        public static readonly DependencyProperty LittleMarkLengthProperty =
            DependencyProperty.Register("LittleMarkLengthProperty", typeof(int), typeof(RulerControl),
            new UIPropertyMetadata(5));

        public int LittleMarkLength
        {
            get => (int)GetValue(LittleMarkLengthProperty);
            set => SetValue(LittleMarkLengthProperty, value);
        }
        #endregion
        #region DepencyProperty StartValueProperty
        public static readonly DependencyProperty StartValueProperty =
            DependencyProperty.Register("StartValueProperty", typeof(double), typeof(RulerControl),
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
        
        static RulerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RulerControl), new FrameworkPropertyMetadata(typeof(RulerControl)));
        }
        
        protected override void OnRender(DrawingContext drawingContext)
        {
            RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            var psuedoStartValue = StartValue;
            #region Horizontal Ruler
            if (Orientation == EnumOrientation.Horizontal)
            {
                for (var i = 0; i < ActualWidth / MajorInterval; i++)
                {
                    var ft = new FormattedText((psuedoStartValue * MajorInterval).ToString(CultureInfo.CurrentCulture),
                        CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                        new Typeface("Tahoma"), 10, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                    drawingContext.DrawText(ft, new Point(i * MajorInterval, 0));
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.Red), 1), new Point(i * MajorInterval, MarkLength), new Point(i * MajorInterval,0));
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.Green), 1),
                        new Point(i * MajorInterval + (MajorInterval / 2), MiddleMarkLength),
                        new Point(i * MajorInterval + (MajorInterval / 2), 0));
                    for (var j = 1; j < 10; j++)
                    {
                        if (j == 5)
                        {
                            continue;
                        }
                        drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.Blue), 1),
                        new Point(i * MajorInterval + (((MajorInterval * j) / 10)), LittleMarkLength),
                        new Point(i * MajorInterval + (((MajorInterval * j) / 10)), 0));
                    }
                    psuedoStartValue++;
                }
            }
            #endregion
            #region Vertical Ruler
            else
            {
                psuedoStartValue = StartValue;
                for (var i = 0; i < ActualHeight / MajorInterval; i++)
                {
                    var ft = new FormattedText((psuedoStartValue * MajorInterval).ToString(CultureInfo.CurrentCulture),
                        CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                        new Typeface("Tahoma"), 10, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);
                    drawingContext.DrawText(ft, new Point(0, i * MajorInterval));
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.Red), 1), new Point(MarkLength, i * MajorInterval), new Point(0, i * MajorInterval));
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.Red), 1), new Point(MarkLength, i * MajorInterval), new Point(0, i * MajorInterval));
                    drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.Green), 1),
                        new Point(MiddleMarkLength, i * MajorInterval + (MajorInterval / 2)),
                        new Point(0, i * MajorInterval + (MajorInterval / 2)));
                    for (var j = 1; j < 10; j++)
                    {
                        if (j==5)
                        {
                            continue;
                        }
                        drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.Blue), 1),
                        new Point(LittleMarkLength, i * MajorInterval + (((MajorInterval*j) / 10))),
                        new Point(0, i * MajorInterval + (((MajorInterval*j) / 10))));
                    }
                    psuedoStartValue++;
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

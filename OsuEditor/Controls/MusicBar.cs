using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using OsuEditor.Models;
using OsuEditor.Models.Timings;

namespace OsuEditor.Controls
{
    [TemplatePart(Name = "PART_Line", Type = typeof(Line))]
    public class MusicBar : Slider
    {
        #region DependencyProperty TimingsProperty
        public static readonly DependencyProperty TimingsProperty =
            DependencyProperty.Register("TimingsProperty", typeof(Timing), typeof(MusicBar),
                new FrameworkPropertyMetadata(new Timing(), FrameworkPropertyMetadataOptions.AffectsRender));

        public Timing Timings
        {
            get => (Timing)GetValue(TimingsProperty);
            set => SetValue(TimingsProperty, value);
        }
        #endregion

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //TODO: Draw preview points, SV points, Kiai periods, break periods
        }
    }
}

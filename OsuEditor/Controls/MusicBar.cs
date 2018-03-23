using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OsuEditor.Controls
{
    [TemplatePart(Name = "PART_Line", Type = typeof(Line))]
    public class MusicBar : Slider
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //TODO: Draw preview points, SV points, Kiai periods, break periods
        }
    }
}

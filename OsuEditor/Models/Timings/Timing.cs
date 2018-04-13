using System.Collections.Generic;

namespace OsuEditor.Models.Timings
{
    public class Timing
    {
        public List<double> BeatLength { get; set; }

        public List<int> BeatsPerMeasure { get; set; }

        public List<int> Offset { get; set; }

        public int PreviewPoint { get; set; }
        
        public List<Bookmark> Bookmarks { get; set; }

        public List<Period> BreakPeriods { get; set; }

        public List<Period> KiaiPeriods { get; set; }

        public List<SvPoint> SvPoints { get; set; }

        public Timing()
        {
            BeatLength = new List<double>();
            BeatsPerMeasure = new List<int>();
            Offset = new List<int>();
            PreviewPoint = -1;
            Bookmarks = new List<Bookmark>();
            BreakPeriods = new List<Period>();
            KiaiPeriods = new List<Period>();
            SvPoints = new List<SvPoint>();
        }

        public Timing(double beatLength, int beatsPerMeasure, int offset, int preview)
        {
            BeatLength = new List<double> { beatLength };
            BeatsPerMeasure = new List<int> { beatsPerMeasure };
            Offset = new List<int> {offset};
            PreviewPoint = preview;
            Bookmarks = new List<Bookmark>();
            BreakPeriods = new List<Period>();
            KiaiPeriods = new List<Period>();
            SvPoints = new List<SvPoint>();
        }

        public Timing(List<double> beatLength, List<int> beatsPerMeasure, List<int> offset, int preivewPoint,
            List<Bookmark> bookMarks, List<Period> breaks, List<Period> kiais, List<SvPoint> svPoints)
        {
            BeatLength = new List<double>(beatLength);
            BeatsPerMeasure = new List<int>(beatsPerMeasure);
            Offset = new List<int>(offset);
            PreviewPoint = preivewPoint;
            Bookmarks = new List<Bookmark>(bookMarks);
            BreakPeriods = new List<Period>(breaks);
            KiaiPeriods = new List<Period>(kiais);
            SvPoints = new List<SvPoint>(svPoints);
        }
    }
}

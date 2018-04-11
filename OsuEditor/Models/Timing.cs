using System.Collections.Generic;
using OsuEditor.Util;

namespace OsuEditor.Models
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

        public Timing()
        {
            BeatLength = new List<double>();
            BeatsPerMeasure = new List<int>();
            Offset = new List<int>();
            PreviewPoint = 1500;
            Bookmarks = new List<Bookmark>();
            BreakPeriods = new List<Period>();
            KiaiPeriods = new List<Period>();
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
        }

        public Timing(List<double> beatLength, List<int> beatsPerMeasure, List<int> offset, int preivewPoint, List<Bookmark> bookMarks, List<Period> breaks, List<Period> kiais)
        {
            BeatLength = new List<double>(beatLength);
            BeatsPerMeasure = new List<int>(beatsPerMeasure);
            Offset = new List<int>(offset);
            PreviewPoint = preivewPoint;
            Bookmarks = new List<Bookmark>(bookMarks);
            BreakPeriods = new List<Period>(breaks);
            KiaiPeriods = new List<Period>(kiais);
        }
    }
}

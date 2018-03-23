using System.Collections.Generic;
using OsuEditor.Util;

namespace OsuEditor.Models
{
    public class Timing
    {
        public List<double> BeatLength { get; set; }

        public List<int> BeatsPerMeasure { get; set; }

        public List<int> Offset { get; set; }

        public Timing()
        {
            BeatLength = new List<double> {BpmConverter.BpmToBeat(160), BpmConverter.BpmToBeat(240)};
            BeatsPerMeasure = new List<int> {4, 3};
            Offset = new List<int> {0, 1234};
        }

        public Timing(double beatLength, int beatsPerMeasure, int offset)
        {
            BeatLength = new List<double> { beatLength };
            BeatsPerMeasure = new List<int> { beatsPerMeasure };
            Offset = new List<int> {offset};
        }

        public Timing(List<double> beatLength, List<int> beatsPerMeasure, List<int> offset)
        {
            BeatLength = new List<double>(beatLength);
            BeatsPerMeasure = new List<int>(beatsPerMeasure);
            Offset = new List<int>(offset);
        }
    }
}

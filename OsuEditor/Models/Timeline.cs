using System.Collections.Generic;
using OsuEditor.Util;

namespace OsuEditor.Models
{
    public class Timeline
    {
        public List<double> BeatLength { get; set; }

        public List<int> BeatsPerMeasure { get; set; }

        public List<int> Offset { get; set; }

        public Timeline()
        {
            BeatLength = new List<double> {BpmConverter.BpmToBeat(160)};
            BeatsPerMeasure = new List<int> {4};
            Offset = new List<int> {0};
        }

        public Timeline(double beatLength, int beatsPerMeasure, int offset)
        {
            BeatLength = new List<double> { beatLength };
            BeatsPerMeasure = new List<int> { beatsPerMeasure };
            Offset = new List<int> {offset};
        }

        public Timeline(List<double> beatLength, List<int> beatsPerMeasure, List<int> offset)
        {
            BeatLength = new List<double>(beatLength);
            BeatsPerMeasure = new List<int>(beatsPerMeasure);
            Offset = new List<int>(offset);
        }
    }
}

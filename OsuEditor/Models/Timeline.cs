using System.Collections.Generic;
using OsuEditor.Util;

namespace OsuEditor.Models
{
    public class Timeline
    {
        public List<double> BeatLength { get; set; }

        public List<int> BeatsPerMeasure { get; set; }

        public int BeatSnap { get; set; }

        public Timeline()
        {
            BeatLength = new List<double> {BpmConverter.BpmToBeat(600)};
            BeatsPerMeasure = new List<int> {4};
            BeatSnap = 4;
        }

        public Timeline(double beatLength, int beatsPerMeasure, int beatSnap)
        {
            BeatLength = new List<double> { beatLength };
            BeatsPerMeasure = new List<int> { beatsPerMeasure };
            BeatSnap = beatSnap;
        }

        public Timeline(List<double> beatLength, List<int> beatsPerMeasure, int beatSnap)
        {
            BeatLength = new List<double>(beatLength);
            BeatsPerMeasure = new List<int>(beatsPerMeasure);
            BeatSnap = beatSnap;
        }
    }
}

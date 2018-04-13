namespace OsuEditor.Models.Timings
{
    public class SvPoint
    {
        public int Offset { get; set; }

        public double Bpm { get; set; }

        public int Rate { get; set; }

        public SvPoint()
        {
            Offset = 0;
            Bpm = 0;
            Rate = 100;
        }

        public SvPoint(int offset, double bpm, int rate)
        {
            Offset = offset;
            Bpm = bpm;
            Rate = rate;
        }

        public SvPoint(SvPoint prevSvPoint)
        {
            Offset = prevSvPoint.Offset;
            Bpm = prevSvPoint.Bpm;
            Rate = prevSvPoint.Rate;
        }
    }
}

namespace OsuEditor.Models
{
    public class TimingMark
    {
        public int Offset { get; set; }

        public double Bpm { get; set; }

        public int SpeedRate { get; set; }

        public int Measure { get; set; }

        public DefaultHitSound HitSound { get; set; }

        public int Volume { get; set; }

        public bool Kiai { get; set; }

        public bool Preview { get; set; }

        public Bookmark Bookmark { get; set; }
        
        public TimingMark()
        {
            Offset = 0;
            Bpm = 0;
            SpeedRate = 100;
            Measure = 4;
            HitSound = DefaultHitSound.Normal;
            Volume = 100;
            Kiai = false;
            Preview = false;
            Bookmark = null;
        }

        public TimingMark(int offset, double bpm, int rate, int measure, DefaultHitSound hitSound, int volume,
            bool kiai, bool preview, Bookmark bookmark)
        {
            Offset = offset;
            Bpm = bpm;
            SpeedRate = rate;
            Measure = measure;
            HitSound = hitSound;
            Volume = volume;
            Kiai = kiai;
            Preview = preview;
            Bookmark = new Bookmark(bookmark);
        }

        public TimingMark(TimingMark prevMark)
        {
            Offset = prevMark.Offset;
            Bpm = prevMark.Bpm;
            SpeedRate = prevMark.SpeedRate;
            Measure = prevMark.Measure;
            HitSound = prevMark.HitSound;
            Volume = prevMark.Volume;
            Kiai = prevMark.Kiai;
            Preview = prevMark.Preview;
            Bookmark = new Bookmark(prevMark.Bookmark);
        }
    }
}

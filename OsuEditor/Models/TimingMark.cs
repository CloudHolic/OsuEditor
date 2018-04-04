namespace OsuEditor.Models
{
    public class TimingMark
    {
        public int Offset { get; set; }

        public bool SpeedChange { get; set; }

        public double Bpm { get; set; }

        public int SpeedRate { get; set; }

        public bool MeasureChange { get; set; }

        public int Measure { get; set; }

        public bool HitSoundChange { get; set; }

        public DefaultHitSound HitSound { get; set; }

        public int Volume { get; set; }

        public bool Kiai { get; set; }

        public bool Preview { get; set; }

        public bool BookMarkChange { get; set; }

        public Bookmark Bookmark { get; set; }
        
        public TimingMark()
        {
            Offset = 0;
            SpeedChange = false;
            Bpm = 120;
            SpeedRate = 100;
            MeasureChange = false;
            Measure = 4;
            HitSoundChange = false;
            HitSound = DefaultHitSound.Normal;
            Volume = 100;
            Kiai = false;
            Preview = false;
            BookMarkChange = false;
            Bookmark = new Bookmark();
        }

        public TimingMark(TimingMark prevMark)
        {
            Offset = prevMark.Offset;
            SpeedChange = false;
            Bpm = prevMark.Bpm;
            SpeedRate = prevMark.SpeedRate;
            MeasureChange = false;
            Measure = prevMark.Measure;
            HitSoundChange = false;
            HitSound = prevMark.HitSound;
            Volume = prevMark.Volume;
            Kiai = prevMark.Kiai;
            Preview = prevMark.Preview;
            BookMarkChange = false;
            Bookmark = new Bookmark(prevMark.Bookmark);
        }
    }
}

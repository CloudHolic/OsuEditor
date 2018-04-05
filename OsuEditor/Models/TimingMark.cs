using System.ComponentModel;

namespace OsuEditor.Models
{
    public class TimingMark : INotifyPropertyChanged
    {
        #region int Offset
        private int _offset;
        public int Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                OnPropertyChanged(nameof(Offset));
            }
        }
        #endregion

        #region bool SpeedChange
        private bool _speedChange;
        public bool SpeedChange
        {
            get => _speedChange;
            set
            {
                _speedChange = value;
                OnPropertyChanged(nameof(SpeedChange));
            }
        }
        #endregion

        #region bool NewBase
        private bool _newBase;
        public bool NewBase
        {
            get => _newBase;
            set
            {
                _newBase = value;
                OnPropertyChanged(nameof(NewBase));
            }
        }
        #endregion

        #region double BPM
        private double _bpm;
        public double Bpm
        {
            get => _bpm;
            set
            {
                _bpm = value;
                OnPropertyChanged(nameof(Bpm));
            }
        }
        #endregion

        #region int SpeedRate
        private int _speedRate;
        public int SpeedRate
        {
            get => _speedRate;
            set
            {
                _speedRate = value;
                OnPropertyChanged(nameof(SpeedRate));
            }
        }
        #endregion

        #region bool MeasureChange
        private bool _measureChange;
        public bool MeasureChange
        {
            get => _measureChange;
            set
            {
                _measureChange = value;
                OnPropertyChanged(nameof(MeasureChange));
            }
        }
        #endregion

        #region int Measure
        private int _measure;
        public int Measure
        {
            get => _measure;
            set
            {
                _measure = value;
                OnPropertyChanged(nameof(Measure));
            }
        }
        #endregion

        #region bool HitSoundChange
        private bool _hitSoundChange;
        public bool HitSoundChange
        {
            get => _hitSoundChange;
            set
            {
                _hitSoundChange = value;
                OnPropertyChanged(nameof(HitSoundChange));
            }
        }
        #endregion

        #region DefaultHitSound HitSound
        private DefaultHitSound _hitSound;
        public DefaultHitSound HitSound
        {
            get => _hitSound;
            set
            {
                _hitSound = value;
                OnPropertyChanged(nameof(HitSound));
            }
        }
        #endregion

        #region int Volume
        private int _volume;
        public int Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }
        #endregion

        #region bool Kiai
        private bool _kiai;
        public bool Kiai
        {
            get => _kiai;
            set
            {
                _kiai = value;
                OnPropertyChanged(nameof(Kiai));
            }
        }
        #endregion

        #region bool Preview
        private bool _preview;
        public bool Preview
        {
            get => _preview;
            set
            {
                _preview = value;
                OnPropertyChanged(nameof(Preview));
            }
        }
        #endregion

        #region bool BookMarkChange
        private bool _bookMarkChange;
        public bool BookMarkChange
        {
            get => _bookMarkChange;
            set
            {
                _bookMarkChange = value;
                OnPropertyChanged(nameof(BookMarkChange));
            }
        }
        #endregion

        #region Bookmark Bookmark
        private Bookmark _bookmark;
        public Bookmark Bookmark
        {
            get => _bookmark;
            set
            {
                _bookmark = value;
                OnPropertyChanged(nameof(Bookmark));
            }
        }
        #endregion

        public TimingMark()
        {
            Offset = 0;
            SpeedChange = true;
            NewBase = true;
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
            NewBase = prevMark.NewBase;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

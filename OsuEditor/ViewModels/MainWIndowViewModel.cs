using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using OsuEditor.Commands;
using OsuEditor.CustomExceptions;
using OsuEditor.Events;

namespace OsuEditor.ViewModels
{
    public class MainWIndowViewModel : ViewModelBase, IEvent<BeatSnapEvent>
    {
        #region Properties
        public bool IsComposeTab
        {
            get { return Get(() => IsComposeTab); }
            set { Set(() => IsComposeTab, value); }
        }

        public int Snap
        {
            get { return Get(() => Snap); }
            set { Set(() => Snap, value); }
        }

        public double Zoom
        {
            get { return Get(() => Zoom); }
            set { Set(() => Zoom, value); }
        }

        public double SongLength
        {
            get { return Get(() => SongLength); }
            set { Set(() => SongLength, value); }
        }

        public double CurrentPosition
        {
            get { return Get(() => CurrentPosition); }
            set { Set(() => CurrentPosition, value); }
        }

        public object BodyContent
        {
            get { return Get(() => BodyContent); }
            set { Set(() => BodyContent, value); }
        }

        public string BeatSnapText
        {
            get { return Get(() => BeatSnapText); }
            set { Set(() => BeatSnapText, value); }
        }

        public double BeatValue
        {
            get { return Get(() => BeatValue); }
            set { Set(() => BeatValue, value); }
        }

        public int PlayRate
        {
            get { return Get(() => PlayRate); }
            set { Set(() => PlayRate, value); }
        }
        #endregion

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private double _oldTime;

        public MainWIndowViewModel()
        {
            Zoom = 5.0;
            Snap = 4;
            SongLength = 100000;
            PlayRate = 100;
            BeatSnapText = $"1/{Snap}";
            BeatValue = BeatSnapToSlider(Snap);
            
            _timer.Interval = TimeSpan.FromMilliseconds((double)1000 / 144);
            _timer.Tick += (sender, args) =>
            {
                var curTime = _stopWatch.Elapsed.TotalMilliseconds;
                CurrentPosition += (curTime - _oldTime) * PlayRate / 100;
                _oldTime = curTime;

                if (CurrentPosition > SongLength)
                {
                    CurrentPosition = SongLength;
                    _timer.Stop();
                }

                EventBus.Instance.Publish(new CurPositionEvent {CurPosition = CurrentPosition});
            };

            EventBus.Instance.RegisterHandler(this);
        }

        private static double BeatSnapToSlider(int beatSnap)
        {
            var beatSnapList = new[] { 1, 2, 3, 4, 6, 8, 12, 16, 24, 32 }.ToList();
            var sliderValueList = new[] { 0, 2, 3, 4, 6, 8, 10, 12, 14, 16 };

            var index = beatSnapList.FindIndex(x => x == beatSnap);
            if (index > -1 && index < 10)
                return sliderValueList[index];

            throw new InvalidValueException();
        }

        public void HandleEvent(BeatSnapEvent e)
        {
            Snap = e.Snap;
            BeatSnapText = $"1/{e.Snap}";
        }
        
        #region Commands
        public ICommand PlayCommand
        {
            get
            {
                return Get(() => PlayCommand, new RelayCommand(() =>
                {
                    if (_timer.IsEnabled)
                    {
                        _timer.Stop();
                        _stopWatch.Stop();
                        CurrentPosition = 0;
                    }

                    _oldTime = 0;
                    _timer.Start();
                    _stopWatch.Start();
                }));
            }
        }

        public ICommand PauseCommand
        {
            get
            {
                return Get(() => PauseCommand, new RelayCommand(() =>
                {
                    if (_timer.IsEnabled)
                    {
                        _timer.Stop();
                        _stopWatch.Stop();
                    }
                    else
                    {
                        _oldTime = 0;
                        _timer.Start();
                        _stopWatch.Start();
                    }
                }));
            }
        }

        public ICommand StopCommand
        {
            get
            {
                return Get(() => StopCommand, new RelayCommand(() =>
                {
                    if (_timer.IsEnabled)
                    {
                        _timer.Stop();
                        _stopWatch.Stop();
                    }
                    CurrentPosition = 0;
                    EventBus.Instance.Publish(new CurPositionEvent { CurPosition = CurrentPosition });
                }));
            }
        }

        public ICommand TestCommand
        {
            get
            {
                return Get(() => TestCommand, new RelayCommand(() =>
                {
                    //TODO: Implement Test.
                }));
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using OsuEditor.Commands;
using OsuEditor.CustomExceptions;
using OsuEditor.Events;
using OsuParser;
using OsuParser.Structures;

namespace OsuEditor.ViewModels
{
    public class MainWIndowViewModel : ViewModelBase
    {
        #region Properties
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

        public int PlayRate
        {
            get { return Get(() => PlayRate); }
            set { Set(() => PlayRate, value); }
        }

        public Beatmap CurrentMap
        {
            get { return Get(() => CurrentMap); }
            set { Set(() => CurrentMap, value); }
        }
        #endregion

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private double _oldTime;

        public MainWIndowViewModel()
        {
            CurrentMap = Parser.CreateBeatmap();
            PlayRate = 100;
            SongLength = 100000;
            CurrentMap.Edit.BeatDivisor = 4;
            CurrentMap.Edit.TimelineZoom = 5.0;

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
                        _stopWatch.Reset();
                        CurrentPosition = 0;
                        EventBus.Instance.Publish(new CurPositionEvent { CurPosition = CurrentPosition });
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
                        _stopWatch.Reset();
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

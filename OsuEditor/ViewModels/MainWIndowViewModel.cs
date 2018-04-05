using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;
using OsuEditor.Commands;
using OsuEditor.Contents;
using OsuEditor.Events;
using OsuEditor.Models;
using OsuEditor.Util;
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

        public ObservableCollection<TimingMark> TimingMarks
        {
            get { return Get(() => TimingMarks); }
            set { Set(() => TimingMarks, value); }
        }

        public TimingMark CurrentTiming
        {
            get { return Get(() => CurrentTiming); }
            set { Set(() => CurrentTiming, value); }
        }
        #endregion

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private double _oldTime;

        private readonly ICustomDialogManager _dialogManager;

        public MainWIndowViewModel()
        {
            CurrentMap = Parser.CreateBeatmap();
            PlayRate = 100;
            SongLength = 100000;
            CurrentMap.Edit.BeatDivisor = 4;
            CurrentMap.Edit.TimelineZoom = 5.0;

            TimingMarks = new ObservableCollection<TimingMark> {new TimingMark()};
            CurrentTiming = TimingMarks[0];

            _timer.Interval = TimeSpan.FromMilliseconds((double) 1000 / 144);
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

            _dialogManager = new CustomDialogManager(new MetroDialogSettings
            {
                AnimateShow = false,
                AnimateHide = false
            });
        }

        #region Commands
        public ICommand AddTimingMarkCommand
        {
            get
            {
                return Get(() => AddTimingMarkCommand, new RelayCommand(() =>
                {
                    if (TimingMarks.Count == 0)
                    {
                        TimingMarks.Add(new TimingMark {Offset = (int) CurrentPosition});
                        CurrentTiming = TimingMarks[0];
                        return;
                    }

                    //  Find previous TimingMark
                    TimingMark prevMark = null;
                    foreach (var timing in TimingMarks)
                        if (CurrentPosition >= timing.Offset)
                            prevMark = timing;

                    if (prevMark == null)
                        prevMark = TimingMarks.First();

                    var newTiming = new TimingMark(prevMark)
                    {
                        Offset = (int) CurrentPosition,
                        Preview = false,
                        Bookmark = new Bookmark()
                    };

                    TimingMarks.Add(newTiming);
                    TimingMarks = new ObservableCollection<TimingMark>(TimingMarks.OrderBy(x => x.Offset));
                    CurrentTiming = newTiming;
                }));
            }
        }

        public ICommand DeleteTimingMarkCommand
        {
            get
            {
                return Get(() => DeleteTimingMarkCommand, new RelayCommand(() =>
                {
                    TimingMarks.Remove(CurrentTiming);
                }));
            }
        }

        public ICommand GroupTimingMarkCommand
        {
            get
            {
                return Get(() => GroupTimingMarkCommand, new RelayCommand(() =>
                {
                    //TODO: Add group command.
                }));
            }
        }

        public ICommand UngroupTimingMarkCommand
        {
            get
            {
                return Get(() => UngroupTimingMarkCommand, new RelayCommand(() =>
                {
                    //TODO: Add ungroup command.
                }));
            }
        }

        public ICommand UseCurrentTimeCommand
        {
            get
            {
                return Get(() => UseCurrentTimeCommand, new RelayCommand(() =>
                {
                    if (CurrentTiming == null)
                        return;

                    CurrentTiming.Offset = (int) CurrentPosition;
                }));
            }
        }

        public ICommand InitialCommand
        {
            get
            {
                return Get(() => InitialCommand, new RelayCommand(async () =>
                {
                    var initSettingView = new InitialSettingView(new InitSettings
                    {
                        Mp3Path = string.Empty,
                        Mode = (PlayMode) CurrentMap.Gen.Mode,
                        Keys = CurrentMap.Gen.Mode == 3 ? (int) CurrentMap.Diff.CircleSize : 4,
                        SpecialStyle = CurrentMap.Gen.SpecialStyle
                    });

                    var result = await _dialogManager.ShowDialogAsync<InitSettings>(initSettingView);
                    if (result != null)
                    {
                        //TODO: Copy MP3 file to the basic mapset directory.

                        CurrentMap.Gen.Mode = (int) result.Mode;
                        if (CurrentMap.Gen.Mode == 3)
                        {
                            CurrentMap.Diff.CircleSize = result.Keys;
                            CurrentMap.Gen.SpecialStyle = result.SpecialStyle;
                        }

                        RaisePropertyChanged(nameof(CurrentMap));
                    }
                }));
            }
        }

        public ICommand EditorCommand
        {
            get
            {
                return Get(() => EditorCommand, new RelayCommand(async () =>
                {
                    var editorSettingView = new EditorSettingView(new EditorSettings());
                    await _dialogManager.ShowDialogAsync<EditorSettings>(editorSettingView);
                }));
            }
        }

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

using System;
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
    public class MainWindowViewModel : ViewModelBase
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

        public bool OffsetErrorOccurred
        {
            get { return Get(() => OffsetErrorOccurred); }
            set { Set(() => OffsetErrorOccurred, value); }
        }
        #endregion

        private readonly DispatcherTimer _playTimer = new DispatcherTimer();
        public DispatcherTimer ErrorTimer { get; } = new DispatcherTimer();
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private double _oldTime;

        private readonly ICustomDialogManager _dialogManager;

        public MainWindowViewModel()
        {
            CurrentMap = Parser.CreateBeatmap();
            PlayRate = 100;
            SongLength = 200000;
            CurrentMap.Edit.BeatDivisor = 4;
            CurrentMap.Edit.TimelineZoom = 5.0;

            TimingMarks = new ObservableCollection<TimingMark> {new TimingMark()};
            CurrentTiming = TimingMarks[0];
            OffsetErrorOccurred = false;

            _playTimer.Interval = TimeSpan.FromMilliseconds((double) 1000 / 144);
            _playTimer.Tick += (sender, args) =>
            {
                var curTime = _stopWatch.Elapsed.TotalMilliseconds;
                CurrentPosition += (curTime - _oldTime) * PlayRate / 100;
                _oldTime = curTime;

                if (CurrentPosition > SongLength)
                {
                    CurrentPosition = SongLength;
                    CurrentPosition = SongLength;
                    _playTimer.Stop();
                }

                EventBus.Instance.Publish(new CurPositionEvent {CurPosition = CurrentPosition});
            };

            ErrorTimer.Interval = TimeSpan.FromSeconds(2);
            ErrorTimer.Tick += (sender, args) =>
            {
                OffsetErrorOccurred = false;
                ErrorTimer.Stop();
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
                    var normalizeCurrentOffset = (int)Math.Round(CurrentPosition);

                    if (TimingMarks.Count == 0)
                    {
                        TimingMarks.Add(new TimingMark {Offset = normalizeCurrentOffset});
                        CurrentTiming = TimingMarks[0];
                        EventBus.Instance.Publish(new CurrentTimingChangedEvent());
                        return;
                    }
                    
                    //  Find previous TimingMark
                    TimingMark prevMark = null;
                    foreach (var timing in TimingMarks)
                    {
                        if (normalizeCurrentOffset == timing.Offset)
                        {
                            CurrentTiming = timing;
                            return;
                        }
                        if (normalizeCurrentOffset > timing.Offset)
                            prevMark = timing;
                    }

                    if (prevMark == null)
                        prevMark = TimingMarks.First();

                    var newTiming = new TimingMark(prevMark)
                    {
                        Offset = normalizeCurrentOffset,
                        Preview = false,
                        Bookmark = new Bookmark()
                    };

                    TimingMarks.Add(newTiming);
                    TimingMarks = new ObservableCollection<TimingMark>(TimingMarks.OrderBy(x => x.Offset));
                    CurrentTiming = newTiming;
                    EventBus.Instance.Publish(new CurrentTimingChangedEvent
                    {
                        Bpm = CurrentTiming.Bpm,
                        Offset = CurrentTiming.Offset
                    });
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

        public ICommand UseCurrentTimeCommand
        {
            get
            {
                return Get(() => UseCurrentTimeCommand, new RelayCommand(() =>
                {
                    if (CurrentTiming == null)
                        return;

                    var normalizeCurrentOffset = (int) Math.Round(CurrentPosition);

                    foreach (var timing in TimingMarks)
                    {
                        if (timing == CurrentTiming)
                            continue;

                        if (timing.Offset == normalizeCurrentOffset)
                        {
                            OffsetErrorOccurred = true;
                            if (ErrorTimer.IsEnabled)
                                ErrorTimer.Stop();
                            ErrorTimer.Start();

                            return;
                        }
                    }

                    CurrentTiming.Offset = (int) Math.Round(CurrentPosition);
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
                    if (_playTimer.IsEnabled)
                    {
                        _playTimer.Stop();
                        _stopWatch.Reset();
                        CurrentPosition = 0;
                        EventBus.Instance.Publish(new CurPositionEvent { CurPosition = CurrentPosition });
                    }

                    _oldTime = 0;
                    _playTimer.Start();
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
                    if (_playTimer.IsEnabled)
                    {
                        _playTimer.Stop();
                        _stopWatch.Stop();
                    }
                    else
                    {
                        _oldTime = 0;
                        _playTimer.Start();
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
                    if (_playTimer.IsEnabled)
                    {
                        _playTimer.Stop();
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
